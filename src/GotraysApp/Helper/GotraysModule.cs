using BlazorComponent.JSInterop;
using Microsoft.JSInterop;

namespace GotraysApp.Helper;

internal class GotraysModule : JSModule
{
    public GotraysModule(IJSRuntime js) : base(js,"/js/gotrays.js")
    {
    }

    public async ValueTask ScrollTo(string id)
    {
        await InvokeVoidAsync("scrollTo", id);
    }
}
