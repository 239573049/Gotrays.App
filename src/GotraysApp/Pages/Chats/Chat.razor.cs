using Force.DeepCloner;
using GotraysApp.Enums;
using Microsoft.AspNetCore.Components;

namespace GotraysApp.Pages.Chats;

public partial class Chat
{
    private ChatType chatType { get; set; }

    private string Value;

    [Parameter]
    [SupplyParameterFromQuery]
    public int ChatType { get => (int)chatType; set => chatType = (ChatType)value; }

    private SettingDto Setting { get; set; } = new();

    public List<ChatMessageDto> ChatMessages { get; set; } = new List<ChatMessageDto>();

    protected override async Task OnInitializedAsync()
    {
        Setting = await StorageService.GetSetting();
        ChatMessages = await FreeSql.Select<ChatMessageDto>()
            .Where(x => x.Type == chatType)
            .OrderBy(x => x.CreatedTime)
            .ToListAsync();
    }

    private void OnReturn()
    {
        NavigationManager.NavigateTo("/");
    }

    private async Task ClearRecord()
    {
        await FreeSql.Select<ChatMessageDto>()
            .Where(x => x.Type == chatType)
            .ToDelete()
            .ExecuteAffrowsAsync();

        ChatMessages.Clear();
    }

    private async Task OnClick()
    {

        var user = new ChatMessageDto()
        {
            Id = Guid.NewGuid(),
            Chat = false,
            Avatar = "https://cdn.masastack.com/stack/images/website/masa-blazor/lists/2.png",
            CreatedTime = DateTime.Now,
            Message = Value,
            Name = "Token",
            Type = chatType
        };

        await FreeSql.Insert<ChatMessageDto>()
            .AppendData(user)
            .ExecuteIdentityAsync();

        ChatMessages.Add(user);

        if (chatType == Enums.ChatType.SD)
        {

            var chat = new ChatMessageDto()
            {
                Id = Guid.NewGuid(),
                Chat = true,
                Avatar = "https://blog-simple.oss-cn-shenzhen.aliyuncs.com/ai.png",
                CreatedTime = DateTime.Now,
                Message="图片生成中，请等待大约15秒",
                Name = "智能助手",
                Type = chatType
            };

            ChatMessages.Add(chat);

            await GotraysModule.ScrollTo("scrool");

            var result = await ApiClientHelper.ImageAI(Value);
            chat.Message = $"data:image/jpeg;base64,{result}";
            _ = InvokeAsync(StateHasChanged);

            await FreeSql.Insert<ChatMessageDto>()
                .AppendData(chat)
                .ExecuteIdentityAsync();

            await Task.Delay(50);

            Value = string.Empty;

            await GotraysModule.ScrollTo("scrool");
        }
        else
        {
            try
            {
                var messages = new List<object>();

                if (!string.IsNullOrEmpty(Setting.Role))
                {
                    messages.Add(new
                    {
                        role = "system",
                        content = Setting.Role
                    });
                }

                if (Setting.IsAbove)
                {
                    // 获取ChatMessage的最后几条
                    var chatMessage = ChatMessages
                        .Where(x => x.Chat)
                        .OrderByDescending(x => x.CreatedTime)
                        .Take(Setting.MaxAbove)
                        .ToList();

                    foreach (var item in chatMessage)
                    {
                        messages.Add(new
                        {
                            role = item.Chat ? "assistant" : "user",
                            content = item.Message
                        });
                    }
                }

                messages.Add(new
                {
                    role = "user",
                    content = Value
                });


                await GotraysModule.ScrollTo("scrool");


                var chat = new ChatMessageDto()
                {
                    Id = Guid.NewGuid(),
                    Chat = true,
                    Avatar = "https://blog-simple.oss-cn-shenzhen.aliyuncs.com/ai.png",
                    CreatedTime = DateTime.Now,
                    Name = "智能助手",
                    Type = chatType
                };

                ChatMessages.Add(chat);

                await ApiClientHelper.CreateChatGptClient(new
                {
                    model = chatType == Enums.ChatType.ChatGPT3_5 ? "gpt-3.5-turbo" : "gpt-3.5-turbo-16k",
                    Setting.max_tokens,
                    Setting.temperature,
                    stream = true,
                    messages
                }, async result =>
                {
                    chat.Message += result;
                    _ = InvokeAsync(StateHasChanged);
                    await GotraysModule.ScrollTo("scrool");
                });

                await FreeSql.Insert<ChatMessageDto>()
                    .AppendData(chat)
                    .ExecuteIdentityAsync();

                Value = string.Empty;
            }
            catch (Exception)
            {

                throw;
            }
        }

    }


}
