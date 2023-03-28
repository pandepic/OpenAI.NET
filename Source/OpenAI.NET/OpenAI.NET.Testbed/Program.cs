using OpenAINET;
using OpenAINET.Testbed;

Console.WriteLine("Enter your API key:");

var apiKey = Console.ReadLine();

var test = new TextStreamedTest(apiKey, OpenAIModelType.TextDavinci003);
Console.WriteLine($"Test started with model: {test.ModelType}");

while (true)
{
    var message = Console.ReadLine();

    try
    {
        await test.AddUserInput(message);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Failed, send an empty message to try again.");
        Console.WriteLine(ex.ToString());
    }
}
