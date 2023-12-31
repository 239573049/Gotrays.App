﻿
namespace GotraysApp.Pages.Authoritys;

public partial class Login
{
    private LoginPayloadDto PayloadDto = new();

    private bool IsDisabled
    {
        get
        {
            if (PayloadDto.Account == null || PayloadDto.Password == null)
            {
                return true;
            }
            return PayloadDto.Account.Length < 5 || PayloadDto.Password.Length < 5;
        }
    }

    public async Task OnLogin()
    {
        try
        {
            var token = await UserService.Login(PayloadDto);
            await SecureStorage.Default.SetAsync("token", token);

            await PopupService.EnqueueSnackbarAsync(new SnackbarOptions
            {
                Title = "登录成功",
                Content = "欢迎回来",
                Type = BlazorComponent.AlertTypes.Success
            });
            await Task.Delay(2000);
            NavigationManager.NavigateTo("/");
        }
        catch (Exception)
        {
            await PopupService.EnqueueSnackbarAsync(new SnackbarOptions
            {
                Title = "登录失败",
                Content = "账号或密码错误",
                Type = BlazorComponent.AlertTypes.Error
            });
        }

    }
}
