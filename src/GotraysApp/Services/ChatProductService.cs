namespace GotraysApp.Services;
public class ChatProductService : CallerBase
{
    public ChatProductService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
    {
    }

    public async Task<List<AIRoleSettingDto>> GetAIRoleSetting()
    {
        return await GetAsync<List<AIRoleSettingDto>>("v1/ChatProducts/AIRoleSetting");
    }
}
