using OpenAINET.Chat.DTO;

namespace OpenAINET.Chat;

public abstract class MessageContent
{
    public virtual int GetTokenCount(SharpToken.GptEncoding encoding) => 0;
    
    public abstract ChatAPIRequestContent CreateAPIRequestContent();
}
