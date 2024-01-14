using OpenAINET.Chat.DTO;

namespace OpenAINET.Chat;

public abstract class MessageContent
{
    public abstract ChatAPIRequestContent CreateAPIRequestContent();
}
