using OpenAINET.Chat.DTO;

namespace OpenAINET.Chat;

public class ChatMessageAssistant : ChatMessage
{
    public string Content { get; set; }
    
    public ChatMessageAssistant() : base(ChatMessageRole.Assistant) { }

    public ChatMessageAssistant(string content, string name = null) : base(ChatMessageRole.Assistant)
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