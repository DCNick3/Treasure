using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blazor.Extensions;
using Blazor.Extensions.Canvas;
using Blazor.Extensions.Canvas.Canvas2D;
using Microsoft.AspNetCore.Components;
using Treasure.GameLogic;
using Treasure.GameLogic.State;
using Treasure.GameLogic.State.Field;
using Treasure.GameLogic.Tiles;

namespace KladWeb
{
    public class TreasureRenderer
    {
        private readonly ElementReference _textureAtlas;
        
        private readonly BECanvasComponent _fieldCanvas;
        private readonly BECanvasComponent _wallsCanvas;
        private readonly BECanvasComponent _playersCanvas;
        
        private Canvas2DContext _fieldCtx = null!;
        private Canvas2DContext _wallsCtx = null!;
        private Canvas2DContext _playersCtx = null!;

        private const int AtlasWidth = 5;
        
        private const string FieldColor = "#6daa2c";
        private const string SwampColor = "#6d892c";
        private const string WaterColor = "#0089f0";
        private const string FieldTextColor = "#000000";
        private static readonly string[] PlayerColors = { "#ff0000", "#0000ff", "#00ff00" };

        private GameState? _prevGameState = null;
        private ImmutableArray<Tile>? _prevTiles = null;
        private (ImmutableList<WallType>, ImmutableList<WallType>)? _prevWalls = null;
        
        
        public TreasureRenderer(BECanvasComponent fieldCanvas, BECanvasComponent wallsCanvas, BECanvasComponent playersCanvas, 
            ElementReference textureAtlas)
        {
            _fieldCanvas = fieldCanvas;
            _wallsCanvas = wallsCanvas;
            _playersCanvas = playersCanvas;
            _textureAtlas = textureAtlas;
        }

        public async Task InitAsync()
        {
            _fieldCtx = await _fieldCanvas.CreateCanvas2DAsync();
            _wallsCtx = await _wallsCanvas.CreateCanvas2DAsync();
            _playersCtx = await _playersCanvas.CreateCanvas2DAsync();
        }

        private async Task RenderField(GameField field)
        {
            await _fieldCtx.BeginBatchAsync();
            
            await _fieldCtx.SetFontAsync("18px serif");
            
            await _fieldCtx.SetFillStyleAsync("black");
            await _fieldCtx.FillRectAsync(0, 0, _fieldCanvas.Width, _fieldCanvas.Height);

            for (var i = 0; i < field.Width; i++)
            for (var j = 0; j < field.Height; j++)
            {
                var c = field.GetTileAt(i, j);
                switch (c)
                {
                    case FieldTile _:
                        await RenderSolidAsync(_fieldCtx, FieldColor, i, j);
                        break;
                    case SwampTile _:
                        await RenderSolidAsync(_fieldCtx, SwampColor, i, j);
                        break;
                    case WaterTile _:
                        await RenderSolidAsync(_fieldCtx, WaterColor, i, j);
                        break;
                    case PortalTile p:
                    {
                        await RenderFromAtlasAsync(_fieldCtx, 2, i, j);
                        var nm = (p.PortalNumber + 1).ToString();
                        await _fieldCtx.SetFillStyleAsync(FieldTextColor);
                        await _fieldCtx.FillTextAsync(nm, i * 64 + 40, j * 64 + 48);
                    }
                        break;
                    case HomeTile h:
                    {
                        await RenderFromAtlasAsync(_fieldCtx, 3, i, j);
                        var nm = (h.PlayerIndex + 1).ToString();
                        await _fieldCtx.SetFillStyleAsync(FieldTextColor);
                        await _fieldCtx.FillTextAsync(nm, i * 64 + 48, j * 64 + 54);
                    }
                        break;
                }
            }
            
            await _fieldCtx.EndBatchAsync();
        }

        private async Task RenderWallsAsync(GameField field)
        {
            await _wallsCtx.BeginBatchAsync();

            await _wallsCtx.ClearRectAsync(0, 0, _wallsCanvas.Width, _wallsCanvas.Height);

            for (var i = 0; i < field.Width; i++)
            for (var j = 0; j < field.Height; j++)
            {

                foreach (var dir in Enum.GetValues(typeof(Direction)).Cast<Direction>())
                {
                    var type = field.GetWallAt(new Point(i, j), dir);
                    if (type != WallType.None)
                    {
                        var rotate = dir switch
                        {
                            Direction.Up => 0,
                            Direction.Right => 1,
                            Direction.Down => 2,
                            Direction.Left => 3,
                            _ => throw new InvalidDataException()
                        };

                        await RenderWallAsync(_wallsCtx, type, i, j, rotate);
                    }
                }
            }

            await _wallsCtx.EndBatchAsync();
        }

        private async Task RenderPlayersAndTreasureAsync(GameState state)
        {
            await _playersCtx.BeginBatchAsync();

            await _playersCtx.ClearRectAsync(0, 0, _playersCanvas.Width, _playersCanvas.Height);
            
            for (var i = 0; i < state.PlayerStates.Length; i++)
                await RenderPlayerAsync(_playersCtx, i, state.PlayerStates[i].Position.X, state.PlayerStates[i].Position.Y);

            await RenderFromAtlasAsync(_playersCtx, 7, state.GameField.TreasurePosition.X, state.GameField.TreasurePosition.Y);
            
            await _playersCtx.EndBatchAsync();
        }
        
        public async Task RenderAsync(GameState gameState)
        {
            if (_prevGameState == gameState)
                return;
            _prevGameState = gameState;

            if (_prevTiles != gameState.GameField.Tiles)
            {
                Console.WriteLine("Re-drawing field");
                _prevTiles = gameState.GameField.Tiles;
                await RenderField(gameState.GameField);
            }

            if (_prevWalls != (gameState.GameField.HorizontalWalls, gameState.GameField.VerticalWalls))
            {
                Console.WriteLine("Re-drawing walls");
                _prevWalls = (gameState.GameField.HorizontalWalls, gameState.GameField.VerticalWalls);
                await RenderWallsAsync(gameState.GameField);
            }

            // changes quite often =)
            await RenderPlayersAndTreasureAsync(gameState);
        }

        private async Task RenderSolidAsync(Canvas2DContext ctx, string fillColor, int i, int j)
        {
            await ctx.SetFillStyleAsync(fillColor);
            await ctx.FillRectAsync(i * 64, j * 64, 64, 64);
        }

        private async Task RenderFromAtlasAsync(Canvas2DContext ctx, int atlasIndex, int i, int j)
        {
            var row = atlasIndex / AtlasWidth;
            var col = atlasIndex % AtlasWidth;
            
            await ctx.DrawImageAsync(_textureAtlas,  64 * col, 64 * row, 64, 64, 64 * i, 64 * j, 64, 64);
        }

        private async Task RenderWallAsync(Canvas2DContext ctx, WallType type, int i, int j, int rotate)
        {
            await ctx.SaveAsync();
            await ctx.SetTransformAsync(1, 0, 0, 1, i * 64 + 32, j * 64 + 32);
            await ctx.RotateAsync((float)(Math.PI * rotate / 2));

            var atlasIndex = type switch
            {
                WallType.Breakable => 9,
                WallType.Unbreakable => 5,
                WallType.Grate => 6,
                _ => throw new InvalidDataException()
            };
            
            var row = atlasIndex / AtlasWidth;
            var col = atlasIndex % AtlasWidth;
            
            await ctx.DrawImageAsync(_textureAtlas,  64 * col, 64 * row, 64, 64, -32, -32, 64, 64);

            await ctx.RestoreAsync();
        }

        private async Task RenderPlayerAsync(Canvas2DContext ctx, int index, int i, int j)
        {
            await ctx.SaveAsync();
            await ctx.BeginPathAsync();
            await ctx.ArcAsync(64 * i + 32, 64 * j + 32, 8, 0, 2 * Math.PI);
                
            await ctx.SetFillStyleAsync(PlayerColors[index]);
            await ctx.FillAsync();

            await ctx.SetLineWidthAsync(1);
            await ctx.SetStrokeStyleAsync("#000000");
            await ctx.StrokeAsync();
            await ctx.ClosePathAsync();
            
            await ctx.RestoreAsync();
        }
    }
}