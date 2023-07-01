using BlazorComponent;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using TouchEventArgs = Microsoft.AspNetCore.Components.Web.TouchEventArgs;

namespace GotraysApp.Shared;
public partial class MainLayout
{

    private string Class;

    [Parameter]
    public RenderFragment ChildContent { get; set; }

    private SettingDto Setting { get; set; } = new SettingDto();

    string Color
    {
        get
        {
            if (AppBars.IndexOf(selectModel) == 0) return "blue-grey";
            if (AppBars.IndexOf(selectModel) == 1) return "teal";
            if (AppBars.IndexOf(selectModel) == 2) return "brown";
            if (AppBars.IndexOf(selectModel) == 3) return "indigo";
            return "blue-grey";
        }
    }


    private readonly List<AppBarDto> AppBars = new();

    private AppBarDto selectModel;

    protected override async Task OnInitializedAsync()
    {
        string token = await SecureStorage.Default.GetAsync("token");
        if (string.IsNullOrEmpty(token))
        {
            NavigationManager.NavigateTo("/login", true);
        }

        AppBars.Add(new AppBarDto("Chat", "/", "mdi-chat-processing-outline"));
        AppBars.Add(new AppBarDto("角色列表", "/role", "mdi-party-popper"));
        AppBars.Add(new AppBarDto("工具", "/tools", "mdi-tools"));
        AppBars.Add(new AppBarDto("我的", "/my", "mdi-account-circle-outline"));

        // 默认选择的导航标签
        selectModel = AppBars[0];


        _ = Task.Run(async () =>
        {
            await FreeSql.Select<ChatMessageDto>().CountAsync();
        });
    }
    /// <summary>
    /// 导航栏跳转
    /// </summary>
    /// <param name="appBar"></param>
    private void GoHref(AppBarDto appBar)
    {
        // 防止重复点击
        if (appBar == selectModel)
        {
            return;
        }

        // 当点击导航的索引大于现在导航时启动滑动效果
        if (AppBars.IndexOf(appBar) > AppBars.IndexOf(selectModel))
        {
            Class = "slide-out-left";
            Task.Run(async () =>
            {
                // 由于特效时间为0.5s 这里是等待特效完成
                await Task.Delay(450);
                NavigationManager.NavigateTo(selectModel.Href);
                Class = "slide-in-right";
                _ = InvokeAsync(StateHasChanged);
            });
        }
        // 当点击导航的索引小于现在导航时启动滑动效果
        else if (AppBars.IndexOf(appBar) < AppBars.IndexOf(selectModel))
        {
            Class = "slide-out-right";
            Task.Run(async () =>
            {
                // 由于特效时间为0.5s 这里是等待特效完成
                await Task.Delay(450);
                NavigationManager.NavigateTo(selectModel.Href);
                Class = "slide-in-left";
                _ = InvokeAsync(StateHasChanged);
            });
        }
        selectModel = appBar;
        NavigationManager.NavigateTo(appBar.Href);
    }


    /// <summary>
    /// 开始X坐标
    /// </summary>
    private double _startX;

    #region 移动端滑动处理

    /// <summary>
    /// 记录开始坐标
    /// </summary>
    /// <param name="args"></param>
    private void TouchStart(TouchEventArgs args)
    {
        var touch = args.ChangedTouches[0];
        _startX = touch.ScreenX;
    }

    private void TouchEnd(TouchEventArgs args)
    {
        var touch = args.ChangedTouches[0];
        Switchover((decimal)touch.ScreenX);
    }

    #endregion

    #region PC滑动处理

    /// <summary>
    /// 记录开始坐标
    /// </summary>
    /// <param name="args"></param>
    private void Mousedown(MouseEventArgs args)
    {
        _startX = args.ScreenX;
    }

    private void Mouseup(MouseEventArgs args)
    {
        Switchover((decimal)args.ScreenX);
    }

    #endregion

    private void Switchover(decimal screenX)
    {
        var index = AppBars.IndexOf(selectModel);
        // 限制过度滑动
        if (index == AppBars.Count || index > AppBars.Count)
        {
            return;
        }

        // 设置滑动最大位限制，达到这个限制才滑动生效
        var size = 200;

        // 需要滑动200才切换 如果开始坐标x大于 当前结束的x坐标往右边切换tab
        if ((decimal)_startX - size > screenX)
        {
            // 如果右边往左边滑动 当前索引是当前最大数量的话不需要切换
            if (index == AppBars.Count - 1)
            {
                return;
            }
            selectModel = AppBars[index + 1];
            Class = "slide-out-left";

            Task.Run(async () =>
            {
                // 由于特效时间为0.5s 这里是等待特效完成
                await Task.Delay(450);
                NavigationManager.NavigateTo(selectModel.Href);
                Class = "slide-in-right";
                _ = InvokeAsync(StateHasChanged);
            });
        }
        else if ((decimal)_startX + size < screenX)
        {
            // 如果左边往右边滑动 当前索引是0的话不需要切换
            if (index == 0)
            {
                return;
            }
            selectModel = AppBars[index - 1];
            Class = "slide-out-right";
            Task.Run(async () =>
            {
                // 由于特效时间为0.5s 这里是等待特效完成
                await Task.Delay(450);
                NavigationManager.NavigateTo(selectModel.Href);
                Class = "slide-in-left";
                _ = InvokeAsync(StateHasChanged);
            });
        }
    }
}
