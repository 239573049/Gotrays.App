using BlazorComponent;

namespace GotraysApp.Pages.Mys;

public partial class My
{
    private GetUserDto userDto = new ();

    private DayDosageDto dayDosage = new();

    private AppInfoDto AppInfo = new ();

    private bool UpdateDisplay = false;

    protected override async Task OnInitializedAsync()
    {
        userDto = await UserService.GetGetUser();

        dayDosage = await UserService.GetDayDosage();
    }

    private async Task UpdateAsync()
    {
        AppInfo = await AppService.GetAsync();

        if (AppInfo.Versions != Constant.Versions)
        {
            UpdateDisplay = true;

            return;
        }
        else
        {
            await PopupService.EnqueueSnackbarAsync(new SnackbarOptions
            {
                Title = "恭喜你，当前是最新版本！",
                Content = AppInfo.Message,
                Type = AlertTypes.Success,
                Timeout = 3000,
            });
        }
    }

    private  void OnExit()
    {
        SecureStorage.Default.Remove("token");
        NavigationManager.NavigateTo("/login");
    }
}
