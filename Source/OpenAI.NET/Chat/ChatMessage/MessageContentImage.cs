using OpenAINET.Chat.DTO;

namespace OpenAINET.Chat;

public abstract class MessageContentImage : MessageContent
{
    public MessageContentImageDetail Detail { get; set; }
    public MessageContentImageType ImageType { get; set; }

    internal ChatAPIRequestImageContentDetailType GetAPIRequestDetail()
    {
        return Detail switch
        {
            MessageContentImageDetail.Low => ChatAPIRequestImageContentDetailType.low,
            MessageContentImageDetail.High => ChatAPIRequestImageContentDetailType.high,
            _ => throw new System.NotImplementedException(),
        };
    }

    internal string GetImageTypeString()
    {
        return ImageType switch
        {
            MessageContentImageType.JPG => "image/jpeg",
            MessageContentImageType.PNG => "image/png",
            MessageContentImageType.WEBP => "image/webp",
            MessageContentImageType.GIF => "image/gif",
            _ => throw new System.NotImplementedException(),
        };
    }
}
