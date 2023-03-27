using OpenAINET.Chat;

Console.WriteLine("Enter your API key:");

var apiKey = Console.ReadLine();

var conversation = new ChatConversation(OpenAINET.OpenAIModelType.GPT3_5Turbo);
var chatAPI = new ChatAPI(apiKey, conversation);
using var streamedAPI = new ChatAPIStreamed(apiKey, conversation);

streamedAPI.OnTokenReceived += (token) =>
{
    Console.Write(token);
};

streamedAPI.OnMessageComplete += (message) =>
{
    Console.WriteLine();
};

streamedAPI.OnError += (error) =>
{
    Console.WriteLine($"Error: {error}");
};

streamedAPI.OnException += (ex) =>
{
    Console.WriteLine($"Exception: {ex}");
};

Console.WriteLine($"Chat started with model: {conversation.ModelType}");

while (true)
{
    var message = Console.ReadLine();

    if (!string.IsNullOrWhiteSpace(message))
        conversation.AddMessage(ChatMessage.FromUser(message));

    try
    {
        #region Non streamed
        //var newMessages = await chatAPI.GetResponse();

        //foreach (var newMessage in newMessages)
        //    Console.WriteLine($"{newMessage.Role}: {newMessage.Message}");

        //Console.WriteLine($"Total tokens: {conversation.TotalTokens}");
        #endregion

        #region Streamed
        streamedAPI.StartStreamingResponse();

        while (streamedAPI.IsStreamingResponse) { }
        #endregion
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Failed, send an empty message to try again.");
        Console.WriteLine(ex.ToString());
    }
}