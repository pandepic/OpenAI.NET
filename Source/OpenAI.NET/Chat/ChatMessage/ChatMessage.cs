using System.Collections.Generic;
using OpenAINET.Chat.DTO;

namespace OpenAINET.Chat;

public enum ChatMessageRole
{
    System,
    User,
    Assistant,
}

public enum MessageContentImageDetail
{
    Low,
    High,
}

public enum MessageContentImageType
{
    JPG,
    PNG,
    WEBP,
    GIF,
}

public abstract class ChatMessage
{
    public ChatMessageRole Role { get; set; }
    public string? Name { get; set; }
    public int? Tokens { get; set; }
    public decimal? EstimatedCost { get; set; }

    public ChatMessage() { }

    public ChatMessage(ChatMessageRole role)
    {
        Role = role;
    }

    public abstract BaseChatAPIRequestMessage CreateAPIRequestMessage();

    public virtual int GetTokenCount(SharpToken.GptEncoding encoding) => 0;

    internal string GetRoleString()
    {
        return Role switch
        {
            ChatMessageRole.System => "system",
            ChatMessageRole.User => "user",
            ChatMessageRole.Assistant => "assistant",
            _ => throw new System.NotImplementedException(),
        };
    }

    public static ChatMessage FromSystem(string message, string name = null)
        => new ChatMessageSystem(message, name);

    public static ChatMessage FromUser(string message, string name = null)
        => new ChatMessageUser(message, name);
    public static ChatMessage FromUser(List<MessageContent> content, string name = null)
        => new ChatMessageUser(content, name);

    public static ChatMessage FromAssistant(string message, string name = null)
        => new ChatMessageAssistant(message, name);
}
