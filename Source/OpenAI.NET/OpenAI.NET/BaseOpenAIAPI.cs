using System;
using System.Collections.Generic;
using System.Text;

namespace OpenAINET
{
    public abstract class BaseOpenAIAPI : BaseHTTPAPI
    {
        public readonly string APIPath;
        public readonly string APIKey;

        public BaseOpenAIAPI(string apiKey, string apiPath)
        {
            APIKey = apiKey;
            APIPath = apiPath;
        }
    }
}
