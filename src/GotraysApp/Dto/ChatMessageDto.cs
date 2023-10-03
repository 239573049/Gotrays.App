using FreeSql.DataAnnotations;

namespace GotraysApp.Dto;

public class ChatMessageDto
{
    [Column(IsPrimary = true)]
    public Guid Id { get; set; }
    
    /// <summary>
    /// 发送内容
    /// </summary>
    [Column(StringLength = 50000)]
    public string Message { get; set; }

    /// <summary>
    /// 发送时间
    /// </summary>
    public DateTime CreatedTime { get; set; }

    /// <summary>
    /// 是否Chat发送
    /// </summary>
    public bool Chat{ get; set; }
}
