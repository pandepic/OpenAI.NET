using System.Collections.Generic;
using OpenAINET.Chat.DTO;

namespace OpenAINET.Chat;

public class ChatMessageUser : ChatMessage
{
    public List<MessageContent> Content { get; set; }

    public ChatMessageUser() : base(ChatMessageRole.User) { }

    public ChatMessageUser(string content) : base(ChatMessageRole.User)
    {
        Content = new() { new MessageContentText(content) };
    }

    public ChatMessageUser(List<MessageContent> content) : base(ChatMessageRole.User)
    {
        Content = content;
    }

    public override BaseChatAPIRequestMessage CreateAPIRequestMessage()
    {
        var request = new ChatAPIRequestMessage()
        {
            role = GetRoleString(),
            content = new(),
        };

        foreach (var item in Content)
            request.content.Add(item.CreateAPIRequestContent());

        return request;
    }
}
