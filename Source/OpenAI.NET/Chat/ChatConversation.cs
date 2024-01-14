using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using OpenAINET.Chat.DTO;

namespace OpenAINET.Chat;

public class ChatConversation
{
    public ChatAPI ChatAPI { get; protected set; }

    public OpenAIModelType ModelType { get; protected set; }
    public List<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
    public int? TotalTokens { get; set; }

    public float Temperature { get; protected set; } = 1f;
    public float TopP { get; protected set; } = 1f;

    public ChatConversation(ChatAPI chatAPI, OpenAIModelType modelType)
    {
        ChatAPI = chatAPI;
        ModelType = modelType;

        if (!ChatAPI.SupportedModels.Contains(modelType))
            throw new ArgumentException($"{modelType} is not supported by the Chat API", nameof(modelType));
    }

    public void AddMessage(ChatMessage message)
    {
        Messages.Add(message);
    }

    public OpenAIModel GetModel()
    {
        return OpenAIModel.Models[ModelType];
    }

    public void SetTemperature(float temperature)
    {
        if (temperature < 0f || temperature > 2f)
            throw new ArgumentException($"Temperature must be between 0 and 2", nameof(temperature));

        Temperature = temperature;
    }

    public void SetTopP(float topP)
    {
        if (topP < 0f || topP > 1f)
            throw new ArgumentException($"TopP must be between 0 and 1", nameof(topP));

        TopP = topP;
    }

    public ChatAPIRequest CreateAPIRequest(int? maxTokens = null)
    {
        var request = new ChatAPIRequest()
        {
            model = GetModel().ModelString,
            temperature = Temperature,
            top_p = TopP,
            max_tokens = maxTokens,
            messages = new(),
        };

        foreach (var message in Messages)
            request.messages.Add(message.CreateAPIRequestMessage());

        return request;
    }

    public async Task<ChatMessage> GetNextAssistantMessageAsync(
        int? maxTokens = null,
        TimeSpan? timeout = null)
    {
        var request = CreateAPIRequest(maxTokens);
        var jsonFormatting = Formatting.None;

        ChatMessage message = null;

#if DEBUG
        jsonFormatting = Formatting.Indented;
#endif

        var serializerSettings = new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = jsonFormatting,
            Converters = new List<JsonConverter>() { new StringEnumConverter() },
        };

        var response = await ChatAPI.GetRawResponseAsync(request, timeout, serializerSettings);

        if (response.error != null)
        {
            throw new Exception(
                $"OpenAI API Error: {response.error}");
        }

        if (response.choices != null && response.choices.Count > 0)
        {
            var choice = response.choices[0];
            message = ChatMessage.FromAssistant(choice.message.content);

            Messages.Add(message);

            if (response.usage != null)
            {
                var model = GetModel();

                var promptCost = model.InputPricePer1kTokens * (response.usage.prompt_tokens / 1000m);
                var responseCost = model.OutputPricePer1kTokens * (response.usage.completion_tokens / 1000m);

                message.EstimatedCost = promptCost + responseCost;
                message.Tokens = response.usage.completion_tokens;
            }
        }

        if (response.usage != null)
            TotalTokens = response.usage.total_tokens;

        return message;
    }
}
