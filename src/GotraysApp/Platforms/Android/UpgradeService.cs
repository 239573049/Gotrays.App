using Android.Content;
using Android.OS;
using GotraysApp.Services;
using Application = Android.App.Application;

using Uri = Android.Net.Uri;

namespace GotraysApp;

/// <summary>
/// Android 更新App实现
/// </summary>
public class UpgradeService : IUpgradeService
{
    private readonly HttpClient _client;

    public UpgradeService(IHttpClientFactory httpClientFactory)
    {
        _client = httpClientFactory.CreateClient(nameof(UpgradeService));
    }

    public void InstallNewVersion()
    {
        var file = $"{FileSystem.AppDataDirectory}/{"com.token.gotraysapp.apk"}";

        var apkFile = new Java.IO.File(file);

        var intent = new Intent(Intent.ActionView);
        // 判断Android版本
        if (Build.VERSION.SdkInt >= BuildVersionCodes.N)
        {
            //给临时读取权限
            intent.SetFlags(ActivityFlags.GrantReadUriPermission);
            var uri = FileProvider.GetUriForFile(Application.Context, "com.token.gotraysapp.fileprovider", apkFile);
            // 设置显式 MIME 数据类型
            intent.SetDataAndType(uri, "application/vnd.android.package-archive");
        }
        else
        {
            intent.SetDataAndType(Uri.FromFile(new Java.IO.File(file)), "application/vnd.android.package-archive");
        }
        //指定以新任务的方式启动Activity
        intent.AddFlags(ActivityFlags.NewTask);

        //激活一个新的Activity
        Application.Context.StartActivity(intent);
    }

    public async Task DownloadFileAsync(string url, Action<long, long> action)
    {
        var req = new HttpRequestMessage(new HttpMethod("GET"), url);
        var response = _client.SendAsync(req, HttpCompletionOption.ResponseHeadersRead).Result;
        var allLength = response.Content.Headers.ContentLength;
        await using var stream = await response.Content.ReadAsStreamAsync();
        var file = $"{FileSystem.AppDataDirectory}/{"com.token.gotraysapp.apk"}";
        await using var fileStream = new FileStream(file, FileMode.Create);
        var buffer = new byte[1024 * 500];
        var readLength = 0;
        int length;
        while ((length = await stream.ReadAsync(buffer)) != 0)
        {
            readLength += length;
            action(readLength, allLength!.Value);
            // 写入到文件
            fileStream.Write(buffer, 0, length);
        }
    }
}
