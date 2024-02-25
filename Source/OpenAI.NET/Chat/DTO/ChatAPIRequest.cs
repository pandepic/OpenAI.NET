using System.Collections.Generic;

namespace OpenAINET.Chat.DTO;

public enum ChatAPIRequestContentType
{
    text,
    image_url,
}

public enum ChatAPIRequestImageContentDetailType
{
    low,
    high,
}

public abstract class BaseChatAPIRequestMessage
{
    public string role { get; set; }
    public string? name { get; set; }
}

public class ChatAPIRequestMessageBasic : BaseChatAPIRequestMessage
{
    public string content { get; set; }
}

public class ChatAPIRequestMessage : BaseChatAPIRequestMessage
{
    public List<ChatAPIRequestContent> content { get; set; }
}

public abstract class ChatAPIRequestContent
{
    public ChatAPIRequestContentType type { get; set; }

    public ChatAPIRequestContent() { }

    public ChatAPIRequestContent(ChatAPIRequestContentType type)
    {
        this.type = type;
    }
}

public class ChatAPIRequestTextContent : ChatAPIRequestContent
{
    public string text { get; set; }

    public ChatAPIRequestTextContent() : base(ChatAPIRequestContentType.text) { }
}

public class ChatAPIRequestImageContentURL
{
    public string url { get; set; }
    public ChatAPIRequestImageContentDetailType detail { get; set; }
}

public class ChatAPIRequestImageContent : ChatAPIRequestContent
{
    public ChatAPIRequestImageContentURL image_url { get; set; }

    public ChatAPIRequestImageContent() : base(ChatAPIRequestContentType.image_url) { }
}

//https://platform.openai.com/docs/api-reference/chat/create
public class ChatAPIRequest
{
    public string model { get; set; }
    public List<BaseChatAPIRequestMessage> messages { get; set; }
    public bool stream { get; set; }
    public float temperature { get; set; } = 0.5f; // 0 to 2
    public float top_p { get; set; } = 1f; // 0 to 1
    public int n { get; set; } = 1;
    public string[] stop { get; set; }
    public float presence_penalty { get; set; } // between -2 and 2
    public float frequency_penalty { get; set; } // between -2 and 2
    public int? seed { get; set; }
    public string user { get; set; }
    public int? max_tokens { get; set; }
}
