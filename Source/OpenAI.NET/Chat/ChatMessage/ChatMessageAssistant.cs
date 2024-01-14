using OpenAINET.Chat.DTO;

namespace OpenAINET.Chat;

public class ChatMessageAssistant : ChatMessage
{
    public string Content { get; set; }
    
    public ChatMessageAssistant() : base(ChatMessageRole.Assistant) { }

    public ChatMessageAssistant(string content) : base(ChatMessageRole.Assistant)
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