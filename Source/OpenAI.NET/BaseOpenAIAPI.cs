namespace OpenAINET;

public abstract class BaseOpenAIAPI : BaseHTTPAPI
{
    public readonly string APIKey;

    public BaseOpenAIAPI(string apiKey)
    {
        APIKey = apiKey;
    }
}
