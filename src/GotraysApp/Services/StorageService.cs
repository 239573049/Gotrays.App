using System.Text.Json;

namespace GotraysApp.Services;

public class StorageService
{
    private SettingDto? Setting;

    public async Task SetSetting(SettingDto dto)
    {
        Setting = dto;

        await SecureStorage.Default.SetAsync("Setting", JsonSerializer.Serialize(dto));
    }

    public async Task<SettingDto> GetSetting()
    {
        if (Setting != null)
        {
            return Setting;
        }
        try
        {
            var setting = await SecureStorage.Default.GetAsync("Setting");

            if (!string.IsNullOrEmpty(setting))
            {
                Setting = JsonSerializer.Deserialize<SettingDto>(setting);
            }
            else
            {
                Setting = new SettingDto();
                await SecureStorage.Default.SetAsync("Setting", JsonSerializer.Serialize(Setting));
            }
        }
        catch (Exception)
        {
            Setting = new SettingDto();
        }
        return Setting;
    }
}
