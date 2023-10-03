using GotraysApp.Extensions;
using GotraysApp.Services;
using Microsoft.Extensions.Logging;

namespace GotraysApp;
public static class MauiProgram
{
    public static MauiApp App;

    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.AddMauiBlazorWebView();
        builder.Services.AddMasaBlazor();
        builder.Services.AddHttpClient();
        builder.Services.AddGotraysApp();
        builder.Services.AddScoped((services) =>
        {

            var fileInfo = new FileInfo(Path.Combine(FileSystem.AppDataDirectory, "app.db"));

            if (fileInfo.Directory?.Exists == false)
            {
                fileInfo.Directory.Create();
            }
            
            return new FreeSql.FreeSqlBuilder()
             .UseConnectionString(FreeSql.DataType.Sqlite, 
             $"Data Source={fileInfo.FullName};")
             .UseMonitorCommand(cmd => Console.WriteLine($"Sql：{cmd.CommandText}")) //监听SQL语句
             .UseAutoSyncStructure(true) //自动同步实体结构到数据库，FreeSql不会扫描程序集，只有CRUD时才会生成表。
             .Build();
        });

#if ANDROID
        builder.Services.AddSingleton<IUpgradeService, UpgradeService>();
#elif WINDOWS
        builder.Services.AddSingleton<IUpgradeService, UpgradeService>();
#endif

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        App = builder.Build();

        return App;
    }
}
