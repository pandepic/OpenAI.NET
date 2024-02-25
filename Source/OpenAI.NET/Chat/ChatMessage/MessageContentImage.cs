using System;
using OpenAINET.Chat.DTO;

namespace OpenAINET.Chat;

public abstract class MessageContentImage : MessageContent
{
    public const double HIGH_DETAIL_SCALED_WIDTH = 2048;
    public const double HIGH_DETAIL_SCALED_HEIGHT = 2048;
    public const double HIGH_DETAIL_SCALED_SHORTEST_SIDE = 768;
    public const double HIGH_DETAIL_TILE_WIDTH = 512;
    public const double HIGH_DETAIL_TILE_HEIGHT = 512;
    public const int HIGH_DETAIL_TILE_TOKENS = 170;
    public const int IMAGE_BASE_TOKENS = 85;
    
    public MessageContentImageDetail Detail { get; set; }
    public MessageContentImageType ImageType { get; set; }
    
    public int Width { get; set; }
    public int Height { get; set; }
    
    public override int GetTokenCount(SharpToken.GptEncoding encoding)
    {
        var tokens = IMAGE_BASE_TOKENS;

        if (Detail == MessageContentImageDetail.High
            && Width > 0 && Height > 0)
        {
            /*
             * As per documentation at https://platform.openai.com/docs/guides/vision
             *
             * High detail images are first scaled to fit within a 2048 x 2048 square, maintaining their aspect ratio.
             * Then, they are scaled such that the shortest side of the image is 768px long.
             * Finally, we count how many 512px squares the image consists of.
             * Each of those squares costs 170 tokens.
             * Another 85 tokens are always added to the final total.
             */
            
            var scaleFactor = 1.0;

            if (Width > HIGH_DETAIL_SCALED_WIDTH || Height > HIGH_DETAIL_SCALED_HEIGHT)
                scaleFactor = Math.Min(HIGH_DETAIL_SCALED_WIDTH / Width, HIGH_DETAIL_SCALED_HEIGHT / Height);

            var scaledWidth = (int)(Width * scaleFactor);
            var scaledHeight = (int)(Height * scaleFactor);

            scaleFactor = 1.0;
            if (scaledWidth > HIGH_DETAIL_SCALED_SHORTEST_SIDE && scaledHeight > HIGH_DETAIL_SCALED_SHORTEST_SIDE)
                scaleFactor = HIGH_DETAIL_SCALED_SHORTEST_SIDE / Math.Min(scaledWidth, scaledHeight);

            scaledWidth = (int)(scaledWidth * scaleFactor);
            scaledHeight = (int)(scaledHeight * scaleFactor);

            var tilesWide = (int)Math.Ceiling(scaledWidth / HIGH_DETAIL_TILE_WIDTH);
            var tilesHigh = (int)Math.Ceiling(scaledHeight / HIGH_DETAIL_TILE_HEIGHT);

            var totalTiles = tilesWide * tilesHigh;

            tokens += totalTiles * HIGH_DETAIL_TILE_TOKENS;
        }
        
        return tokens;
    }

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
