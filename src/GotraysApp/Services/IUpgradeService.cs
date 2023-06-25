namespace GotraysApp.Services;

public interface IUpgradeService
{
    void  InstallNewVersion();

    Task DownloadFileAsync(string url, Action<long, long> action);
}
