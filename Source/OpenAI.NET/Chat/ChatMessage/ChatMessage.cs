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
    public int? Tokens { get; set; }
    public decimal? EstimatedCost { get; set; }

    public ChatMessage() { }

    public ChatMessage(ChatMessageRole role)
    {
        Role = role;
    }

    public abstract BaseChatAPIRequestMessage CreateAPIRequestMessage();

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

    public static ChatMessage FromSystem(string message) => new ChatMessageSystem(message);

    public static ChatMessage FromUser(string message) => new ChatMessageUser(message);
    public static ChatMessage FromUser(List<MessageContent> content) => new ChatMessageUser(content);

    public static ChatMessage FromAssistant(string message) => new ChatMessageAssistant(message);
}
