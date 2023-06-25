using GotraysApp.Enums;

namespace GotraysApp.Pages.Chats;

public partial class Home
{
    private void OnGo(ChatType chat)
    {
        NavigationManager.NavigateTo("/chat?ChatType=" + (int)chat);
    }
}
