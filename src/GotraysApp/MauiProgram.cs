using GotraysApp.Extensions;
using GotraysApp.Services;
using Microsoft.Extensions.Logging;

namespace GotraysApp;
public static class MauiProgram
{
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

            return new FreeSql.FreeSqlBuilder()
             .UseConnectionString(FreeSql.DataType.Sqlite, 
             $"Data Source={Path.Combine(FileSystem.AppDataDirectory, "gotrays.db")};")
                .UseLazyLoading(true)
             .UseAutoSyncStructure(true) //自动同步实体结构到数据库
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

        return builder.Build();
    }
}
