using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using SDL2;
using Treasure.GameLogic;
using Treasure.GameLogic.Controllers;
using Treasure.GameLogic.State;
using Treasure.GameLogic.State.Field;
using Treasure.GameLogic.State.History;
using Treasure.GameLogic.Tiles;

namespace TreasureVisualize
{
    public class App
    {
        private IntPtr _renderer;

        private int Width => _gameController.GameState.GameField.Width;
        private int Height => _gameController.GameState.GameField.Height;

        private const int TileSize = 64;
        private const int AtlasRowSize = 5;

        private int PixelWidth => Width * TileSize;
        private int PixelHeight => Height * TileSize;

        private static readonly (byte, byte, byte) FieldColor = (0x6d,0xaa, 0x2c);
        private static readonly (byte, byte, byte) SwampColor = (0x6d, 0x89, 0x2c);
        private static readonly (byte, byte, byte) WaterColor = (0x00, 0x89, 0xf0);
        private static readonly (byte, byte, byte) FieldTextColor = (0x00, 0x00, 0x00);

        private static readonly (byte, byte, byte)[] PlayerColors = {
            (255, 0, 0),
            (0, 0, 255),
            (0, 255, 0),
            (0, 255, 255),
            (255, 0, 255),
        };
        
        private GameController _gameController;
        private readonly ReusableAwaiter<PlayerAction> _actionPipe = new ReusableAwaiter<PlayerAction>();
        private IntPtr _textureAtlas;
        private IntPtr _font;
        
        private Dictionary<(string, (byte, byte, byte)), IntPtr> _textTextureCache = new Dictionary<(string, (byte, byte, byte)), IntPtr>();
        
        public void Execute()
        {
            var controller = new SdlPlayerController(this);
            var playerCount = 5;
            var @params = new GameFieldParams(
                10, 10, playerCount, 3, 4, 3, 0.1, true);
            var field = new GameFieldGenerator(@params, new Random(42)).Generate();

            var homePositions = field.HomePositions.ToList();
            var p = homePositions[^1];
            homePositions.RemoveAt(homePositions.Count - 1);
            homePositions.Insert(0, p);
            
            var gameState = new GameState(0, field, 
                homePositions.Select(_ => new PlayerState(_)).ToImmutableArray(), null, new GameHistory());
            
            _gameController = new GameController(gameState,
                Enumerable.Repeat(controller, playerCount), new IPlayerNotifier[]
                {
                    new ConsoleNotifier()
                });

            

            Trace.Assert(SDL.SDL_Init(SDL.SDL_INIT_VIDEO | SDL.SDL_INIT_EVENTS) >= 0);
            Trace.Assert(SDL_image.IMG_Init(SDL_image.IMG_InitFlags.IMG_INIT_PNG) == (int)SDL_image.IMG_InitFlags.IMG_INIT_PNG);
            Trace.Assert(SDL_ttf.TTF_Init() == 0);

            Trace.Assert(SDL.SDL_CreateWindowAndRenderer(PixelWidth, PixelHeight, SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN,
                             out var window, out _renderer) == 0);

            _textureAtlas = SDL_image.IMG_LoadTexture(_renderer, "texture_atlas.png");
            if (_textureAtlas == IntPtr.Zero)
                throw new Exception(SDL.SDL_GetError());
            
            _font = SDL_ttf.TTF_OpenFont("LiberationSans-Regular.ttf", 14);
            if (_font == IntPtr.Zero)
                throw new Exception(SDL.SDL_GetError());
            
            var task = _gameController.DoGameLoopAsync(CancellationToken.None)
                .ContinueWith(_ =>
                {
                    if (_.IsCompletedSuccessfully)
                        Console.WriteLine($"Player {_.Result} won");
                    else
                        Console.WriteLine(_.Exception);
                    SDL.SDL_Quit();
                });

            var running = true;
            while (running)
            {
                while (SDL.SDL_PollEvent(out var @event) != 0)
                {
                    Event(@event);
                    
                    if (@event.type == SDL.SDL_EventType.SDL_QUIT)
                    {
                        running = false;
                        break;
                    }
                }

                Render();
                SDL.SDL_Delay(1000 / 30);
            }
            
            SDL.SDL_DestroyWindow(window);
        }

        private void Event(SDL.SDL_Event @event)
        {
            if (@event.type == SDL.SDL_EventType.SDL_KEYDOWN)
            {
                switch (@event.key.keysym.scancode)
                {
                    case SDL.SDL_Scancode.SDL_SCANCODE_W:
                        _actionPipe.TrySetResult(new PlayerAction(PlayerAction.ActionType.Move, Direction.Up));
                        break;
                    case SDL.SDL_Scancode.SDL_SCANCODE_D:
                        _actionPipe.TrySetResult(new PlayerAction(PlayerAction.ActionType.Move, Direction.Right));
                        break;
                    case SDL.SDL_Scancode.SDL_SCANCODE_S:
                        _actionPipe.TrySetResult(new PlayerAction(PlayerAction.ActionType.Move, Direction.Down));
                        break;
                    case SDL.SDL_Scancode.SDL_SCANCODE_A:
                        _actionPipe.TrySetResult(new PlayerAction(PlayerAction.ActionType.Move, Direction.Left));
                        break;
                    case SDL.SDL_Scancode.SDL_SCANCODE_I:
                        _actionPipe.TrySetResult(new PlayerAction(PlayerAction.ActionType.Shoot, Direction.Up));
                        break;
                    case SDL.SDL_Scancode.SDL_SCANCODE_L:
                        _actionPipe.TrySetResult(new PlayerAction(PlayerAction.ActionType.Shoot, Direction.Right));
                        break;
                    case SDL.SDL_Scancode.SDL_SCANCODE_K:
                        _actionPipe.TrySetResult(new PlayerAction(PlayerAction.ActionType.Shoot, Direction.Down));
                        break;
                    case SDL.SDL_Scancode.SDL_SCANCODE_J:
                        _actionPipe.TrySetResult(new PlayerAction(PlayerAction.ActionType.Shoot, Direction.Left));
                        break;
                }
            }
        }

        private void FillRect(int x, int y, int w, int h, (byte, byte, byte) color)
        {
            var rect = new SDL.SDL_Rect
            {
                x = x, y = y,
                w = w, h = h,
            };
            SDL.SDL_SetRenderDrawColor(_renderer, color.Item1, color.Item2, color.Item3, 255);
            SDL.SDL_RenderFillRect(_renderer, ref rect);
        }

        private void FillTile(int i, int j, (byte, byte, byte) color)
        {
            FillRect(i * TileSize, j * TileSize, TileSize, TileSize, color);
        }

        private void DrawTileAtlas(int i, int j, int atlasIndex, double angle)
        {
            var row = atlasIndex / AtlasRowSize;
            var col = atlasIndex % AtlasRowSize;
            
            var src = new SDL.SDL_Rect
            {
                x = col * TileSize, y = row * TileSize,
                w = TileSize, h = TileSize,
            };
            var dst = new SDL.SDL_Rect
            {
                x = i * TileSize, y = j * TileSize,
                w = TileSize, h = TileSize,
            };

            SDL.SDL_RenderCopyEx(_renderer, _textureAtlas, ref src, ref dst, angle, IntPtr.Zero, 
                SDL.SDL_RendererFlip.SDL_FLIP_NONE);
        }

        private void DrawTextAt(int x, int y, string text, (byte, byte, byte) color)
        {
            if (!_textTextureCache.TryGetValue((text, color), out var texture))
            {
                var surf = SDL_ttf.TTF_RenderText_Blended(_font, text, new SDL.SDL_Color
                {
                    r = color.Item1, g = color.Item2, b = color.Item3,
                    a = 255
                });

                SDL_image.IMG_SavePNG(surf, $"text_{text}.png");
                
                texture = SDL.SDL_CreateTextureFromSurface(_renderer, surf);

                _textTextureCache[(text, color)] = texture;
            }

            SDL.SDL_QueryTexture(texture, out var format, out var access, out var width, out var height);


            var dst = new SDL.SDL_Rect
            {
                x = x, y = y,
                w = width, h = height
            };
                
            
            SDL.SDL_RenderCopy(_renderer, texture, IntPtr.Zero, ref dst);
        }

        private void DrawPlayer(int i, int j, int index)
        {
            SDL.SDL_GetTextureColorMod(_textureAtlas, out var r, out var g, out var b);

            var (cr, cg, cb) = PlayerColors[index];
            SDL.SDL_SetTextureColorMod(_textureAtlas, cr, cg, cb);

            DrawTileAtlas(i, j, 8, 0);
            
            SDL.SDL_SetTextureColorMod(_textureAtlas, r, g, b);
        }
        
        private void Render()
        {
            var state = _gameController.GameState;
            var field = state.GameField;
            for (var i = 0; i < field.Width; i++)
            for (var j = 0; j < field.Height; j++)
            {
                var t = field.GetTileAt(i, j);
                switch (t)
                {
                    case FieldTile _:
                        FillTile(i, j, FieldColor);
                        break;
                    case WaterTile _:
                        FillTile(i, j, WaterColor);
                        break;
                    case SwampTile _:
                        FillTile(i, j, SwampColor);
                        break;
                    case PortalTile p:
                    {
                        DrawTileAtlas(i, j, 2, 0);
                        var nm = (p.PortalNumber + 1).ToString();
                        DrawTextAt(i * TileSize + 40, j * TileSize + 34, nm, FieldTextColor);
                        //await ctx.SetFillStyleAsync(FieldTextColor);
                        //await ctx.FillTextAsync(nm, i * 64 + 40, j * 64 + 48);
                    }
                        break;
                    case HomeTile h:
                    {
                        DrawTileAtlas(i, j, 3, 0);
                        var nm = (h.PlayerIndex + 1).ToString();
                        DrawTextAt(i * TileSize + 44, j * TileSize + 40, nm, FieldTextColor);
                        //await ctx.SetFillStyleAsync(FieldTextColor);
                        //await ctx.FillTextAsync(nm, i * 64 + 48, j * 64 + 54);
                    }
                        break;
                }

                foreach (var direction in Enum.GetValues(typeof(Direction)).Cast<Direction>())
                {
                    var rotate = direction switch
                    {
                        Direction.Up => 0,
                        Direction.Right => 90,
                        Direction.Down => 180,
                        Direction.Left => 270,
                        _ => throw new ArgumentOutOfRangeException()
                    };
                    
                    var wall = field.GetWallAt(i, j, direction);
                    switch (wall)
                    {
                        case WallType.None:
                            break;
                        case WallType.Breakable:
                            DrawTileAtlas(i, j, 9, rotate);
                            break;
                        case WallType.Unbreakable:
                            DrawTileAtlas(i, j, 5, rotate);
                            break;
                        case WallType.Grate:
                            DrawTileAtlas(i, j, 6, rotate);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

            DrawTileAtlas(field.TreasurePosition.X, field.TreasurePosition.Y, 7, 0);
            
            foreach (var (player, index) in state.PlayerStates.Zip(Enumerable.Range(0, state.PlayerStates.Length)))
                DrawPlayer(player.Position.X, player.Position.Y, index);

            SDL.SDL_RenderPresent(_renderer);
        }

        public async Task<PlayerAction> GetPlayerActionAsync(int yourIndex)
        {
            var r = await _actionPipe.Reset();
            return r;
        }
    }

    public class SdlPlayerController : IPlayerController
    {
        private readonly App _app;

        public SdlPlayerController(App app)
        {
            _app = app;
        }

        public void OnGameStart(int yourIndex, GameFieldParams fieldParams)
        {
        }

        public Task<PlayerAction> GetActionAsync(int yourIndex, CancellationToken cancellationToken, GameHistory history)
        {
            return _app.GetPlayerActionAsync(yourIndex);
        }
    }
    
    public sealed class ReusableAwaiter<T> : INotifyCompletion
    {
        private Action _continuation;
        private T _result;
        private Exception _exception;

        public bool IsCompleted
        {
            get;
            private set;
        }

        public T GetResult()
        {
            if (_exception != null)
                throw _exception;
            return _result;
        }

        public void OnCompleted(Action continuation)
        {
            if (_continuation != null)
                throw new InvalidOperationException("This ReusableAwaiter instance has already been listened");
            _continuation = continuation;
        }

        /// <summary>
        /// Attempts to transition the completion state.
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool TrySetResult(T result)
        {
            if (!this.IsCompleted)
            {
                this.IsCompleted = true;
                this._result = result;

                _continuation?.Invoke();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Attempts to transition the exception state.
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool TrySetException(Exception exception)
        {
            if (!this.IsCompleted)
            {
                this.IsCompleted = true;
                this._exception = exception;

                _continuation?.Invoke();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Reset the awaiter to initial status
        /// </summary>
        /// <returns></returns>
        public ReusableAwaiter<T> Reset()
        {
            this._result = default(T);
            this._continuation = null;
            this._exception = null;
            this.IsCompleted = false;
            return this;
        }

        public ReusableAwaiter<T> GetAwaiter()
        {
            return this;
        }
    }
}