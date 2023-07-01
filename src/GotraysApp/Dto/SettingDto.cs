namespace GotraysApp.Dto;

public class SettingDto
{
    /// <summary>
    /// 最大Token
    /// </summary>
    public int max_tokens { get; set; } = 1000;

    /// <summary>
    /// 温度
    /// </summary>
    public double temperature { get; set; } = 0;

    /// <summary>
    /// 最大上文
    /// </summary>
    public int MaxAbove { get; set; } = 5;

    /// <summary>
    /// 是否开启上文
    /// </summary>
    public bool IsAbove { get; set; } = false;

    /// <summary>
    /// 角色
    /// </summary>
    public string Role { get; set; }

    /// <summary>
    /// 角色Id
    /// </summary>
    public string RoleId { get; set; }
}
