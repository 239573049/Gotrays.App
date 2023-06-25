# GotraysApp

## 介绍

gotrays-app 一款基于Maui Blazor实现的App，支持ChatGPT聊天，支持图形AI，支持角色定义
支持检查更新。
打造Gotrays生态开源免费。
后续长期开源维护支持

- Maui Blazor
- Masa Blazor

## 功能

支持ChatGPT3.5
支持ChatGPT3.5-16K
支持图片AI模型

## 支持
- [Open666](https://open666.cn) 提供Api服务支持
- [Masa Blazor](https://www.masastack.com/blazor) 使用组件实现界面功能


## 贡献

如果您想参与贡献，请按照一下步骤

1. 对于项目进行`Fork`，拉取代码

```shell
git clone https://github.com/239573049/Gotrays.App.git
```
2. 新建分支，如果是新功能请使用`feature/xxx`，使用`feature`作为前缀，增加`/`在创建分支的时候git会创建目录层级


3. 对于项目进行修改，并且对于修改的部分存在复杂逻辑请对于代码块进行注释，参考下例

```csharp

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

```
4. 新建PR。

## 捐赠

支持微信捐赠，感谢支持

![zs](https://open666.cn/img/zs.jpg)

## 交流

qq技术交流群： 737776595
wx请联系`wk28u9123456789`

