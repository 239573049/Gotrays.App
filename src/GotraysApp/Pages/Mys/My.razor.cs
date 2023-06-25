namespace GotraysApp.Pages.Mys;

public partial class My
{
    private GetUserDto userDto = new GetUserDto();

    private DayDosageDto dayDosage = new();

    protected override async Task OnInitializedAsync()
    {
        userDto = await UserService.GetGetUser();

        dayDosage = await UserService.GetDayDosage();
    }

    private async Task UpdateAsync()
    {
        var info = await AppService.GetAsync();

        if (info.Versions != Constant.Versions)
        {
            await PopupService.EnqueueSnackbarAsync(new SnackbarOptions
            {
                Title = "有新版本更新!新版本:" + info.Versions,
                Content = info.Message,
                Type = BlazorComponent.AlertTypes.Success,
                Timeout = 5000,
            });

            return;
        }
        else
        {
            await PopupService.EnqueueSnackbarAsync(new SnackbarOptions
            {
                Title = "恭喜你，当前是最新版本！",
                Content = info.Message,
                Type = BlazorComponent.AlertTypes.Success,
                Timeout = 5000,
            });
        }
    }

    private async Task OnExit()
    {
        SecureStorage.Default.Remove("token");
        NavigationManager.NavigateTo("/login");
    }
}
