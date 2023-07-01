using BlazorComponent;

namespace GotraysApp.Pages.Mys;

public partial class Setting
{

    private SettingDto SettingDto { get; set; } = new();

    private void OnReturn()
    {
        NavigationManager.NavigateTo("/my");
    }

    protected override async Task OnInitializedAsync()
    {
        SettingDto = await StorageService.GetSetting();

    }

    private async Task OnSave()
    {
        await StorageService.SetSetting(SettingDto);

        await PopupService.EnqueueSnackbarAsync(new SnackbarOptions
        {
            Title = "保存成功！",
            Type = AlertTypes.Success,
            Timeout = 2000,
        });
    }
}
