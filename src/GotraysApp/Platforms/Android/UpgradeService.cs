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
        var response = await _client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);

        var totalBytes = response.Content.Headers.ContentLength.GetValueOrDefault();

        await using var stream = await response.Content.ReadAsStreamAsync();

        var file = $"{FileSystem.AppDataDirectory}/{"com.token.gotraysapp.apk"}";
        await using var fileStream = new FileStream(file, FileMode.Create,
            FileAccess.Write, FileShare.None, 8192, true);

        var totalReadBytes = 0L;
        var buffer = new byte[8192];
        var isMoreToRead = true;
        do
        {
            var read = await stream.ReadAsync(buffer, 0, buffer.Length);
            if (read == 0)
            {
                isMoreToRead = false;
            }
            else
            {
                await fileStream.WriteAsync(buffer, 0, read);

                totalReadBytes += read;
                action.Invoke(totalReadBytes, totalBytes);
            }
        } while (isMoreToRead);

    }
}
