using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using RunProcessAsTask;
using Treasure.GameLogic.State.Field;

namespace Treasure.Checker.Socket
{
    class Program
    {
        class BaseOptions
        {
            [Option('f', "field-file", Required = false)]
            public string? FieldFile { get; set; }
            [Option('s', "field-seed")]
            public int? Seed { get; set; }

            public Random Rnd => Seed != null ? new Random(Seed.Value) : new Random();
            
            public GameField GetGameField()
            {
                var @params = new GameFieldParams(10, 10, 1, 3, 6, 3, 0.1, false);
                
                if (FieldFile != null)
                    return GameField.FromString(@params, File.ReadAllText(FieldFile));
                
                var field = new GameFieldGenerator(@params, Rnd).Generate();
                return field;
            }
        }
        
        [Verb("listen-many")]
        class ListenManyOptions : BaseOptions
        {
            [Option('l', "listen", Required = false)]
            public string ListenEndpoint { get; set; } = "127.177.0.13:56454";

            [Option('t', "io-timeout", Required = false)]
            public int Timeout { get; set; } = 60000;
        }

        [Verb("exec")]
        class ExecOptions : BaseOptions
        {
            [Option('l', "listen", Required = false)]
            public string ListenEndpoint { get; set; } = "127.177.0.13:56454";

            [Value(0)] public string Executable { get; set; } = null!;

            [Option('t', "io-timeout", Required = false)]
            public int Timeout { get; set; } = 4000;
        }

        static async Task Main(string[] args)
        {
            await (await Parser.Default.ParseArguments<ListenManyOptions, ExecOptions>(args)
                .WithParsedAsync<ListenManyOptions>(async o =>
                {
                    async Task ServeAsync(GameField field, System.Net.Sockets.Socket client)
                    {
                        await Console.Error.WriteLineAsync($"{client.RemoteEndPoint} connected");
                        using var comm = new TcpCommunicator(client, o.Timeout);
                        var checker = new Checker(field, comm);
                        try
                        {
                            await checker.CheckAsync();
                            await Console.Error.WriteLineAsync(
                                $"{client.RemoteEndPoint} has won in {checker.GameState.History.Items.Count} moves");
                        }
                        catch (Exception e)
                        {
                            await Console.Error.WriteLineAsync($"{client.RemoteEndPoint} produced an error: {e}");
                        }
                    }
                    
                    var field1 = o.GetGameField();
                    var sock = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    sock.Bind(IPEndPoint.Parse(o.ListenEndpoint));
                    sock.Listen(2);
                    await Console.Error.WriteLineAsync($"Listening on {o.ListenEndpoint}");
                    while (true)
                    {
                        var client = await sock.AcceptAsync();
                        var t = ServeAsync(field1, client);
                    }
                }))
                .WithParsedAsync<ExecOptions>(async o =>
                {
                    bool runtimeError = false;
                    bool timeout = false;
                    try
                    {
                        var field = o.GetGameField();
                        var sock = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Stream,
                            ProtocolType.Tcp);
                        sock.Bind(IPEndPoint.Parse(o.ListenEndpoint));
                        sock.Listen(1);
                        sock.Blocking = false;
                        await Console.Error.WriteLineAsync($"Listening on {o.ListenEndpoint}");

                        var process = Process.Start(o.Executable);
                        var t = new Thread(() =>
                        {
                            Thread.Sleep(10000);
                            timeout = true;
                            process.Kill();
                        }) {IsBackground = true};
                        t.Start();

                        try
                        {
                            var stopWatch = new Stopwatch();
                            stopWatch.Start();
                            System.Net.Sockets.Socket? client = null;
                            while (stopWatch.Elapsed < TimeSpan.FromSeconds(5))
                            {
                                try
                                {
                                    client = sock.Accept();
                                    break;
                                }
                                catch (SocketException e)
                                {
                                    if (e.SocketErrorCode != SocketError.WouldBlock)
                                        throw;
                                }

                                Thread.Sleep(100);
                            }

                            if (client == null)
                                throw new TimeLimit();

                            client.Blocking = true;

                            await Console.Error.WriteLineAsync($"{client.RemoteEndPoint} connected");
                            var comm = new TcpCommunicator(client, o.Timeout);
                            var checker = new Checker(field, comm);
                            await checker.CheckAsync();
                            await Console.Error.WriteLineAsync(
                                $"Finished in {checker.GameState.History.Items.Count} actions");
                            Console.WriteLine(checker.GameState.History.Items.Count);
                        }
                        finally
                        {
                            try
                            {
                                if (!process.HasExited)
                                {
                                    process.Kill();
                                    process.WaitForExit();
                                }
                                else
                                {
                                    if (process.ExitCode != 0)
                                        runtimeError = true;
                                }
                            }
                            catch (InvalidOperationException)
                            { }
                        }
                    }
                    catch (TimeLimit)
                    {
                        if (!timeout && runtimeError)
                            Console.WriteLine("Runtime Error");
                        else
                            Console.WriteLine("Time Limit");
                    }
                    catch (Checker.PresentationError)
                    {
                        Console.WriteLine("Presentation Error");
                    }
                    catch (Exception e)
                    {
                        await Console.Error.WriteLineAsync($"Program produced an error: {e}");
                        throw;
                    }
                });
        }

        class TcpCommunicator : IProgramCommunicator, IDisposable
        {
            private readonly NetworkStream _ns;
            private readonly TextReader _reader;
            private readonly TextWriter _writer;

            public TcpCommunicator(System.Net.Sockets.Socket sock, int timeout)
            {
                sock.ReceiveTimeout = timeout;
                sock.SendTimeout = timeout;
                _ns = new NetworkStream(sock, true);

                _reader = new StreamReader(_ns);
                _writer = new StreamWriter(_ns);
            }
            
            public async Task<string> ReadLineAsync()
            {
                try
                {
                    var line = _reader.ReadLine() ?? throw new TimeLimit();
                    await Console.Error.WriteLineAsync($"> {line}");
                    return line;
                }
                catch (SocketException e)
                {
                    throw new TimeLimit(e);
                }
                catch (TimeLimit)
                {
                    throw;
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine($"Exception: {e}");
                    throw new TimeLimit(e);
                }
            }

            public async Task WriteLineAsync(string value)
            {
                try
                {
                    await Console.Error.WriteLineAsync($"< {value}");
                    _writer.WriteLine(value);
                    _writer.Flush();
                }
                catch (SocketException e)
                {
                    throw new TimeLimit(e);
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine($"Exception: {e}");
                    throw new TimeLimit(e);
                }
            }

            public void Dispose()
            {
                _ns?.Dispose();
            }
        }

        class TimeLimit : Exception
        {
            public TimeLimit() : base("Time limit exceeded")
            {
            }
            

            public TimeLimit(Exception e) : base("Time limit exceeded", e)
            {
            }
        }
    }
}
