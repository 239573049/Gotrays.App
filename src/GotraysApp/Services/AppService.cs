namespace GotraysApp.Services;

public class AppService : CallerBase
{
    public AppService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
    {
    }

    public async Task<AppInfoDto> GetAsync()
    {
        return await GetAsync<AppInfoDto>("v1/Apps/" + Constant.Id);
    }

}
