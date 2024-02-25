using OpenAINET.Chat.DTO;

namespace OpenAINET.Chat;

public class MessageContentImageURL : MessageContentImage
{
    public string ImageURL { get; set; }

    public MessageContentImageURL() { }

    public MessageContentImageURL(string imageURL,
        MessageContentImageDetail detail = MessageContentImageDetail.Low,
        int width = 0,
        int height = 0)
    {
        ImageURL = imageURL;
        Detail = detail;
        
        Width = width;
        Height = height;
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
