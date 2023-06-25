namespace GotraysApp.Pages.Roles;

public partial class Role
{
    private List<AIRoleSettingDto> aIRoleSettingDtos = new ();

    public SettingDto Setting { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        aIRoleSettingDtos = await ChatProductService.GetAIRoleSetting();
        Setting = await StorageService.GetSetting();
    }

    private async Task SetRole(AIRoleSettingDto dto)
    {
        if(Setting.RoleId == dto.Id)
        {
            Setting.Role = null;
            Setting.RoleId = null;
        }
        else
        {
            Setting.Role = dto.Content;
            Setting.RoleId = dto.Id;
        }
        await StorageService.SetSetting(Setting);
    }
}
