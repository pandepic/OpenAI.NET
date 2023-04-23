using System.Text;
using Microsoft.VisualBasic;
using OpenAINET.Chat;
using OpenAINET.Text;

namespace OpenAINET.Testbed
{
    public class TextStreamedTest : Test, IDisposable
    {
        public TextAPIStreamed TextAPIStreamed;
        public StringBuilder History = new();

        public TextStreamedTest(string apiKey, OpenAIModelType modelType) : base(apiKey, modelType)
        {
            TextAPIStreamed = new(ModelType, APIKey);

            TextAPIStreamed.OnTokenReceived += (token) =>
            {
                Console.Write(token);
            };

            TextAPIStreamed.OnResponseComplete += (response) =>
            {
                History.AppendLine(response);
                Console.WriteLine();
            };

            TextAPIStreamed.OnError += (error) =>
            {
                Console.WriteLine($"Error: {error}");
            };

            TextAPIStreamed.OnException += (ex) =>
            {
                Console.WriteLine($"Exception: {ex}");
            };
        }

        public override async Task AddUserInput(string input)
        {
            if (!string.IsNullOrWhiteSpace(input))
                History.AppendLine(input);

            TextAPIStreamed.StartStreamingResponse(History.ToString(), maxTokens: 1000);
            TextAPIStreamed.WaitForResponse();
        }

        public void Dispose()
        {
        }
    }
}
