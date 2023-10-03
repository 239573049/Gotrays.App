using GotraysApp.Services;
using Masa.Blazor;
using System.Text.Json;
using Microsoft.JSInterop;

namespace GotraysApp.Pages.Chats;

public partial class Home
{
    private string value;
    private bool fab;

    private string ContentId = Guid.NewGuid().ToString("N");

    private readonly List<ChatMessageDto> ChatMessages = new();

    public GetUserDto UserInfoDto;
    private DotNetObjectReference<Home> _reference;

    private int page = 1;

    private static readonly MarkdownItAnchorOptions s_anchorOptions = new()
    {
        Level = 1,
        PermalinkClass = "",
        PermalinkSymbol = ""
    };

    protected override async Task OnInitializedAsync()
    {
        UserInfoDto = await UserService.GetGetUser();
        _reference = DotNetObjectReference.Create(this);
        await Load();
    }

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            _ = Task.Run((async () =>
            {
                await Task.Delay(200);
                await DocumentInterop.OnScroll(ContentId, _reference, nameof(OnScroll));
            }));
        }
    }

    /// <summary>
    /// 得到Style
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    private string GetMessageStyle(bool v)
    {
        // 根据状态返回style
        if (v)
        {
            return "float: left;";
        }

        return "float: right;";
    }


    private async Task OnClick()
    {

        var user = new ChatMessageDto()
        {
            Id = Guid.NewGuid(),
            Chat = false,
            CreatedTime = DateTime.Now,
            Message = value,
        };
        ChatMessages.Add(user);

        await Free.Insert(user).ExecuteAffrowsAsync();


        List<object> chatMessage = new List<object>();

        var storage=await StorageService.GetSetting();

        if (!string.IsNullOrWhiteSpace(storage.Role))
        {
            chatMessage.AddRange(ChatMessages.Select(x => new
            {
                role = "system",
                content = storage.Role
            }));
        }

        chatMessage.AddRange(ChatMessages.Select(x => new
        {
            role = x.Chat ? "assistant" : "user",
            content = x.Message
        }));

        chatMessage.Add(new
        {
            role = "user",
            content = value
        });

        value = string.Empty;

        var message = new ChatMessageDto()
        {
            Id = Guid.NewGuid(),
            Chat = true,
            CreatedTime = DateTime.Now,
            Message = string.Empty
        };

        ChatMessages.Add(message);

        // 首次发送需要将滚动条置底
        await DocumentInterop.ScrollToBottom(ContentId);

        await Task.Delay(75);

        var response = await ChatService.HttpRequestRaw("v1/Chats/SendMessage", new
        {
            max_tokens = 1000,
            temperature = 0,
            stream = true,
            messages = chatMessage
        });
        await using var json = await response.Content.ReadAsStreamAsync();
        var strings = JsonSerializer.DeserializeAsyncEnumerable<string>(json);
        try
        {
            await foreach (var str in strings)
            {
                message.Message += str;
                await InvokeAsync(StateHasChanged);
                await DocumentInterop.ScrollToBottom(ContentId);
            }
        }
        catch
        {
        }

        await Free.Insert(message).ExecuteAffrowsAsync();
    }

    private async Task Load()
    {
        var value = (await Free.Select<ChatMessageDto>()
                .OrderByDescending(x => x.CreatedTime)
                .Page(page, 10)
                .ToListAsync())
            .OrderBy(x => x.CreatedTime)
            .ToList();

        page++;

        if (value.Count == 0)
        {
            page--;
            return;
        }

        ChatMessages.InsertRange(0, value);
        _ = InvokeAsync(StateHasChanged);
    }

    private async Task RemoveAll()
    {
        ChatMessages.Clear();
        await Free.Delete<ChatMessageDto>()
            .Where(x => true)
            .ExecuteAffrowsAsync();
    }

    [JSInvokable]
    public async Task OnScroll(double scrollTop)
    {
        if (scrollTop == 0)
        {
            await Load();
        }
    }

    public string ComputeIcon()
    {
        return fab ? "mdi-close" : " mdi-tools";
    }
}
