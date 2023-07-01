using GotraysApp.Services;

namespace GotraysApp;

public class UpgradeService : IUpgradeService
{
    public async Task DownloadFileAsync(string url, Action<long, long> action)
    {
        await Task.CompletedTask;
    }

    public void InstallNewVersion()
    {
        
    }
}
