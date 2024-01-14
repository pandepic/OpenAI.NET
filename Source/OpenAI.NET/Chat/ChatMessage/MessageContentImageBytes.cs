using System;
using OpenAINET.Chat.DTO;

namespace OpenAINET.Chat;

public class MessageContentImageBytes : MessageContentImage
{
    public byte[] Bytes { get; set; }

    public MessageContentImageBytes() { }

    public MessageContentImageBytes(byte[] bytes,
        MessageContentImageDetail detail = MessageContentImageDetail.Low)
    {
        Bytes = bytes;
        Detail = detail;
    }

    public override ChatAPIRequestContent CreateAPIRequestContent()
    {
        var base64 = Convert.ToBase64String(Bytes);

        return new ChatAPIRequestImageContent()
        {
            image_url = new()
            {
                url = $"data:{GetImageTypeString()};base64,{base64}",
                detail = GetAPIRequestDetail(),
            }
        };
    }
}
