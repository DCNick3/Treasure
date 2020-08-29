using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Blazor.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Treasure.Checker;
using Treasure.GameLogic.State.Field;

namespace KladWeb.Shared
{
    public partial class KladComponent
    {
        protected MarkupString TextContents { get; set; }
        protected ElementReference TextContainer { get; set; }
        protected ElementReference TextElement { get; set; }
        protected ElementReference AtlasImageReference { get; set; }
        protected BECanvasComponent FieldCanvasReference = null!;
        protected BECanvasComponent WallsCanvasReference = null!;
        protected BECanvasComponent PlayersCanvasReference = null!;

        private readonly InputAwaiter _checkerInputAwaiter = new InputAwaiter();

        private readonly GameFieldParams _params = new GameFieldParams(10, 10, 1, 
            3, 6, 3, 0.1, false);
        
        private Checker _checker = null!;
        private GameField _initialField = null!;
        private Task _checkerTask = null!;
        private TreasureRenderer _renderer = null!;
        
        private bool _shouldScroll = false;
        private bool _rendered = false;

        [Inject] 
        protected JsInterop JsInterop { get; set; } = null!;
        
        protected override void OnInitialized()
        {
            Restart();
        }


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JsInterop.WaitForImageLoadAsync(AtlasImageReference);
                Console.WriteLine("WaitForImageLoadAsync completed");
                
                _renderer = new TreasureRenderer(FieldCanvasReference, WallsCanvasReference, PlayersCanvasReference, AtlasImageReference);
                await _renderer.InitAsync();
                _rendered = true;

                Console.WriteLine("Rendering...");
                await RenderAsync();
            }

            if (_shouldScroll)
            {
                await JsInterop.ScrollElementIntoView(TextElement, false);
                _shouldScroll = false;
            }
        }

        private void AppendLine(string value)
        {
            TextContents = new MarkupString(TextContents.Value + HttpUtility.HtmlEncode(value) + "<br>");
            _shouldScroll = true;
            StateHasChanged();
        }
        
        protected Task AddStuffAsync()
        {
            AppendLine("<stuff>");
            return Task.CompletedTask;
        }

        protected void MoveUp() => _checkerInputAwaiter.PushInput("Движение вверх");
        protected void MoveDown() => _checkerInputAwaiter.PushInput("Движение вниз");
        protected void MoveLeft() => _checkerInputAwaiter.PushInput("Движение влево");
        protected void MoveRight() => _checkerInputAwaiter.PushInput("Движение вправо");

        protected void ShootUp() => _checkerInputAwaiter.PushInput("Выстрел вверх");
        protected void ShootDown() => _checkerInputAwaiter.PushInput("Выстрел вниз");
        protected void ShootLeft() => _checkerInputAwaiter.PushInput("Выстрел влево");
        protected void ShootRight() => _checkerInputAwaiter.PushInput("Выстрел вправо");

        private async Task RenderAsync()
        {
            if (_rendered)
                await _renderer.RenderAsync(_checker.GameState);
            else
                throw new Exception("Not yet ready to render");
        }

        private void UseField(GameField field)
        {
            _initialField = field;
            TextContents = new MarkupString("");
            _checker = new Checker(field, new KladCommunicator(this));
            _checkerTask = _checker.CheckAsync().ContinueWith(async t =>
            {
                Console.WriteLine("Checker completed");
                if (t.IsFaulted)
                    Console.WriteLine($"We have a problem: {t.Exception}");
                else
                    await RenderAsync();
            });
        }
        
        private void Restart()
        {
            UseField(new GameFieldGenerator(_params, new Random()).Generate());
        }

        private async Task ImportMapAsync()
        {
            var mapString = await JsInterop.Prompt("Введите строку карты");
            if (mapString == null)
                return;
            mapString = mapString.Trim();
            UseField(GameField.FromString(_params, mapString));
        }

        private async Task ExportMapAsync()
        {
            var mapString = _initialField.Stringify().Trim();
            await JsInterop.CopyToClipboard(mapString);
            //await JsInterop.Prompt("Вот строка карты", mapString);
        }
        
        private class InputAwaiter : INotifyCompletion
        {
            private Action? _continuation;
            private string? _result;
            
            public bool IsCompleted { get; private set; }
            
            public void OnCompleted(Action continuation) => _continuation = continuation;

            public void PushInput(string value)
            {
                IsCompleted = true;
                _result = value;
                
                _continuation?.Invoke();
            }

            public InputAwaiter Prepare()
            {
                IsCompleted = false;
                _result = null;
                _continuation = null;
                return this;
            }
            
            public string GetResult() => _result ?? throw new InvalidOperationException();
            public InputAwaiter GetAwaiter() => this;
        }
        
        private class KladCommunicator : IProgramCommunicator
        {
            private readonly KladComponent _kladComponent;

            public KladCommunicator(KladComponent kladComponent)
            {
                _kladComponent = kladComponent;
            }

            public async Task<string> ReadLineAsync()
            {
                if (_kladComponent._rendered)
                    await _kladComponent.RenderAsync();
                var input = await _kladComponent._checkerInputAwaiter.Prepare();
                _kladComponent.AppendLine(input);
                return input;
            }

            public Task WriteLineAsync(string value)
            {
                _kladComponent.AppendLine(value);
                return Task.CompletedTask;
            }
        }
    }
}