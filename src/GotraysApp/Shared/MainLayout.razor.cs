using Microsoft.AspNetCore.Components;

namespace GotraysApp.Shared;
public partial class MainLayout
{
    private string Class;

    [Parameter]
    public RenderFragment ChildContent { get; set; }

    private SettingDto Setting { get; set; } = new SettingDto();
    
    protected override async Task OnInitializedAsync()
    {
        string token = await SecureStorage.Default.GetAsync("token");
        if (string.IsNullOrEmpty(token))
        {
            NavigationManager.NavigateTo("/login", true);
        }

    }

}
