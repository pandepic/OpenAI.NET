using OpenAINET.Chat.DTO;

namespace OpenAINET.Chat;

public class MessageContentImageURL : MessageContentImage
{
    public string ImageURL { get; set; }

    public MessageContentImageURL() { }

    public MessageContentImageURL(string imageURL,
        MessageContentImageDetail detail = MessageContentImageDetail.Low)
    {
        ImageURL = imageURL;
        Detail = detail;
    }

    public override ChatAPIRequestContent CreateAPIRequestContent()
    {
        return new ChatAPIRequestImageContent()
        {
            image_url = new()
            {
                url = ImageURL,
                detail = GetAPIRequestDetail(),
            }
        };
    }
}
