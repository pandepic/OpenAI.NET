using OpenAINET.Chat.DTO;

namespace OpenAINET.Chat;

public class MessageContentText : MessageContent
{
    public string Text { get; set; }

    public MessageContentText() { }

    public MessageContentText(string text)
    {
        Text = text;
    }
    
    public override int GetTokenCount(SharpToken.GptEncoding encoding)
    {
        if (string.IsNullOrEmpty(Text))
            return 0;
        
        return encoding.Encode(Text).Count;
    }

    public override ChatAPIRequestContent CreateAPIRequestContent()
    {
        return new ChatAPIRequestTextContent()
        {
            text = Text,
        };
    }
}
