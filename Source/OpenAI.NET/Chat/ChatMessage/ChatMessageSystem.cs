using OpenAINET.Chat.DTO;

namespace OpenAINET.Chat;

public class ChatMessageSystem : ChatMessage
{
    public string Content { get; set; }

    public ChatMessageSystem() : base(ChatMessageRole.System) { }

    public ChatMessageSystem(string content) : base(ChatMessageRole.System)
    {
        Content = content;
    }

    public override BaseChatAPIRequestMessage CreateAPIRequestMessage()
    {
        return new ChatAPIRequestMessageBasic()
        {
            role = GetRoleString(),
            content = Content
        };
    }
}
