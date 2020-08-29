using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace KladWeb
{
    public class JsInterop
    {
        private const string NamespacePrefix = "KladWeb";
        private readonly IJSRuntime _js;

        public JsInterop(IJSRuntime js)
        {
            _js = js;
        }

        public ValueTask ScrollElementIntoView(ElementReference reference, bool alignToTop) => 
            _js.InvokeVoidAsync($"{NamespacePrefix}.ScrollElementIntoView", reference, alignToTop);

        public ValueTask WaitForImageLoadAsync(ElementReference reference) =>
            _js.InvokeVoidAsync($"{NamespacePrefix}.WaitForImageLoadAsync", reference);

        public ValueTask CopyToClipboard(string text) =>
            _js.InvokeVoidAsync($"{NamespacePrefix}.CopyToClipboard", text);

        public ValueTask<string?> Prompt(string message, string @default = "") => 
            _js.InvokeAsync<string?>("prompt", message, @default);
    }
}