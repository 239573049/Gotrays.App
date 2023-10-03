namespace GotraysApp.Services;
public class ChatProductService : CallerBase
{
    private static List<AIRoleSettingDto> _aiRoleSetting;

    public ChatProductService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
    {
    }

    public async Task<List<AIRoleSettingDto>> GetAIRoleSetting()
    {
        if (_aiRoleSetting == null)
        {
            _aiRoleSetting = await GetAsync<List<AIRoleSettingDto>>("v1/ChatProducts/AIRoleSetting");
        }

        return _aiRoleSetting;
    }
}
