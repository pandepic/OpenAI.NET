using System.Text;
using OpenAINET.Text;

namespace OpenAINET.Testbed
{
    public class TextTest : Test
    {
        public TextAPI TextAPI;
        public StringBuilder History = new();

        public TextTest(string apiKey, OpenAIModelType modelType) : base(apiKey, modelType)
        {
            TextAPI = new(ModelType, APIKey);
        }

        public override async Task AddUserInput(string input)
        {
            if (!string.IsNullOrEmpty(input))
                History.AppendLine(input);

            var (responses, promptTokens, responseTokens, totalTokens) = await TextAPI.GetResponseAsync(History.ToString(), maxTokens: 1000);

            foreach (var response in responses)
            {
                History.AppendLine(response);
                Console.WriteLine(response);
            }

            Console.WriteLine($"Total tokens: {totalTokens}");
        }
    }
}
