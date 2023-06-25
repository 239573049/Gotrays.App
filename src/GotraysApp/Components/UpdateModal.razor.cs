using FreeSql;
using Microsoft.AspNetCore.Components;

namespace GotraysApp.Components;

public partial class UpdateModal
{
    private int Ps { get; set; }

    private long TotalBytesToReceive { get; set; }

    private long BytesReceived { get; set; }

    [Parameter]
    public bool Value { get; set; }

    [Parameter]
    public EventCallback<bool> ValueChanged { get; set; }

    [Parameter]
    public AppInfoDto AppInfo { get; set; } = new ();

    private bool IsUpdate;

    private async Task OnValueChanged(bool value)
    {
        Value = value;
        await ValueChanged.InvokeAsync(Value);
    }

    private async Task OnCancel()
    {
        Value = false;
        await ValueChanged.InvokeAsync(Value);
    }

    private async Task OnSave()
    {
        IsUpdate = true;
        await UpgradeService.DownloadFileAsync(AppInfo.Url, DownloadProgressChanged);
        UpgradeService.InstallNewVersion();
    }
    private void DownloadProgressChanged(long readLength, long allLength)
    {
        InvokeAsync(() =>
        {
            var c = (int)(readLength * 100 / allLength);

            if (c > 0 && c % 1 == 0) //刷新进度为每5%更新一次，过快的刷新会导致页面显示数值与实际不一致
            {
                Ps = c; //下载完成百分比
                BytesReceived = readLength / 1024; //当前已经下载的Kb
                TotalBytesToReceive = allLength / 1024; //文件总大小Kb
                StateHasChanged();
            }
        });
    }
}
