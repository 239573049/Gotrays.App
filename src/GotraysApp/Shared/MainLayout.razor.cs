using BlazorComponent;

namespace GotraysApp.Shared;
public partial class MainLayout
{
    StringNumber value = 0;

    private SettingDto Setting { get; set; } = new SettingDto();

    string color
    {
        get
        {
            if (value == 0) return "blue-grey";
            if (value == 1) return "teal";
            if (value == 2) return "brown";
            if (value == 3) return "indigo";
            return "blue-grey";
        }
    }
    protected override async Task OnInitializedAsync()
    {
        string token = await SecureStorage.Default.GetAsync("token");
        if (string.IsNullOrEmpty(token))
        {
            NavigationManager.NavigateTo("/login", true);
        }

        _ = Task.Run(async () =>
        {
            await FreeSql.Select<ChatMessageDto>().CountAsync();
        });
    }
}
