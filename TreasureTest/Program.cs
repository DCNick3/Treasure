using System;
using System.Collections.Immutable;
using System.Threading;
using Treasure.GameLogic;
using Treasure.GameLogic.Controllers;
using Treasure.GameLogic.State.Field;
using Treasure.GameLogic.Tiles;

namespace TreasureTest
{
    class Program
    {
        static void Fuzz(GameFieldParams @params)
        {
            var rnd = new Random(42);

            for (var i = 0; i < 10000; i++)
            {
                var f = new GameFieldGenerator(@params, rnd).Generate();
                var str = f.Stringify();
                var f1 = GameField.FromString(@params, str);
            }
        }
        
        static void Main(string[] args)
        {
            Fuzz(new GameFieldParams(10, 10, 2, 3, 
                4, 3, 0.1, false));
            Fuzz(new GameFieldParams(10, 10, 2, 3, 
                4, 3, 0.1, true));
            Fuzz(new GameFieldParams(6, 10, 1, 4, 
                4, 3, 0.1, true));
            Fuzz(new GameFieldParams(11, 10, 1, 4, 
                2, 6, 0.1, false));
            
            var @params = new GameFieldParams(10, 10, 2, 3, 
                4, 3, 0.1, true);
            var field = new GameFieldGenerator(@params, new Random(42)).Generate();

            for (var j = 0; j < field.Height * 2 + 1; j++)
            {
                for (var i = 0; i < field.Width * 2 + 1; i++)
                {
                    var c = " ";
                    if (i % 2 == 1 && j % 2 == 1)
                    {
                        var t = field.GetTileAt(i / 2, j / 2);
                        c = t switch
                        {
                            FieldTile _ => ".",
                            SwampTile _ => "W",
                            WaterTile _ => "~",
                            PortalTile _ => "O",
                            HomeTile _ => "^",
                            _ => throw new Exception()
                        };
                        if (field.TreasurePosition.XY == (i / 2, j / 2))
                            c = "K";

                    }
                    else if (j % 2 == 1)
                    {
                        var w = field.GetVerticalWall(i / 2, j / 2);
                        c = w switch
                        {
                            WallType.None => " ",
                            WallType.Breakable => "|",
                            WallType.Unbreakable => "!",
                            _ => throw new Exception()
                        };
                    }
                    else if (i % 2 == 1)
                    {
                        var w = field.GetHorizontalWall(i / 2, j / 2);
                        c = w switch
                        {
                            WallType.None => " ",
                            WallType.Breakable => "-",
                            WallType.Unbreakable => "!",
                            WallType.Grate => "#",
                            _ => throw new Exception()
                        };
                    }

                    Console.Write(c);
                }

                Console.WriteLine();
            }

            Console.WriteLine(field.CheckDefinitelyPassable());

            var game = new GameController(field,
                new IPlayerController[] {new ConsolePlayerController(), new ConsolePlayerController()},
                new IPlayerNotifier[] { new ConsoleNotifier() });
            
            game.DoGameLoopAsync(CancellationToken.None).ContinueWith(winner =>
                Console.WriteLine($"Player {winner.Result + 1} won!"));
        }
    }
}