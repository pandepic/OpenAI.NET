using OpenAINET;
using OpenAINET.Embeddings;
using OpenAINET.Moderation;
using OpenAINET.Testbed;

Console.WriteLine("Enter your API key:");

var apiKey = Console.ReadLine();

// var embeddingsAPI = new EmbeddingsAPI(apiKey);
// var (data, usage) = await embeddingsAPI.CreateEmbeddingVectorAsync("some test string");

//var moderationAPI = new ModerationAPI(apiKey);
//var (model, results) = await moderationAPI.GetResultsAsync("some test string");

//var test = new ChatTest(apiKey, OpenAIModelType.GPT3_5Turbo);
//var test = new ChatStreamedTest(apiKey, OpenAIModelType.GPT3_5Turbo);
//var test = new TextTest(apiKey, OpenAIModelType.TextDavinci003);
//var test = new TextStreamedTest(apiKey, OpenAIModelType.TextDavinci003);

// Console.WriteLine($"Test started with model: {test.ModelType}");
//
// while (true)
// {
//     var message = Console.ReadLine();
//
//     try
//     {
//         await test.AddUserInput(message);
//     }
//     catch (Exception ex)
//     {
//         Console.WriteLine($"Failed, send an empty message to try again.");
//         Console.WriteLine(ex.ToString());
//     }
// }
