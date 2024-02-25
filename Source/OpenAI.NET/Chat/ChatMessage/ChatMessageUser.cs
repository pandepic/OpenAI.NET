using System.Collections.Generic;
using OpenAINET.Chat.DTO;

namespace OpenAINET.Chat;

public class ChatMessageUser : ChatMessage
{
    public List<MessageContent> Content { get; set; }

    public ChatMessageUser() : base(ChatMessageRole.User) { }

    public ChatMessageUser(string content, string name = null) : base(ChatMessageRole.User)
    {
        Name = name;
        Content = new() { new MessageContentText(content) };
    }

    public ChatMessageUser(List<MessageContent> content, string name = null) : base(ChatMessageRole.User)
    {
        Name = name;
        Content = content;
    }

    public override int GetTokenCount(SharpToken.GptEncoding encoding)
    {
        if (Content == null || Content.Count == 0)
            return 0;
        
        var contentTokens = 0;

        foreach (var content in Content)
            contentTokens += content.GetTokenCount(encoding);
        
        return contentTokens;
    }

    public override BaseChatAPIRequestMessage CreateAPIRequestMessage()
    {
        var request = new ChatAPIRequestMessage()
        {
            role = GetRoleString(),
            content = new(),
            name = Name,
        };

        foreach (var item in Content)
            request.content.Add(item.CreateAPIRequestContent());

        return request;
    }
}
