using Microsoft.AspNetCore.Components;

namespace GotraysApp.Components;

public partial class SendMessage
{
    [Parameter]
    public string Value { get; set; }

    [Parameter]
    public EventCallback<string> ValueChanged { get; set; }

    [Parameter]
    public EventCallback OnClick { get; set; }
}
