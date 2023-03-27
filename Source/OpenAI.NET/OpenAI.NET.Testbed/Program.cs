using OpenAINET.Chat;

Console.WriteLine("Enter your API key:");

var apiKey = Console.ReadLine();

var chatAPI = new ChatAPI(apiKey);
var conversation = new ChatConversation(OpenAINET.OpenAIModelType.GPT3_5Turbo);

Console.WriteLine($"Chat started with model: {conversation.ModelType}");

while (true)
{
    var message = Console.ReadLine();

    if (!string.IsNullOrWhiteSpace(message))
        conversation.AddMessage(ChatMessage.FromUser(message));

    try
    {
        var newMessages = await chatAPI.UpdateConversation(conversation);

        foreach (var newMessage in newMessages)
            Console.WriteLine($"{newMessage.Role}: {newMessage.Message}");

        Console.WriteLine($"Total tokens: {conversation.TotalTokens}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Failed, send an empty message to try again.");
        Console.WriteLine(ex.ToString());
    }
}