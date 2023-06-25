namespace GotraysApp.Dto;

public class AppInfoDto
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    /// <summary>
    /// 版本号
    /// </summary>
    public string Versions { get; set; }

    /// <summary>
    /// 下载地址
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// 更新内容
    /// </summary>
    public string Message { get; set; }
}
