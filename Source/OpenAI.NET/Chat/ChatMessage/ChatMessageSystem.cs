using OpenAINET.Chat.DTO;

namespace OpenAINET.Chat;

public class ChatMessageSystem : ChatMessage
{
    public string Content { get; set; }

    public ChatMessageSystem() : base(ChatMessageRole.System) { }

    public ChatMessageSystem(string content, string name = null) : base(ChatMessageRole.System)
    {
        Name = name;
        Content = content;
    }
    
    public override int GetTokenCount(SharpToken.GptEncoding encoding)
    {
        if (string.IsNullOrEmpty(Content))
            return 0;
        
        return encoding.Encode(Content).Count;
    }

    public override BaseChatAPIRequestMessage CreateAPIRequestMessage()
    {
        return new ChatAPIRequestMessageBasic()
        {
            name = Name,
            role = GetRoleString(),
            content = Content
        };
    }
}
