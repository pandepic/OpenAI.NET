using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using OpenAINET.Chat.DTO;

namespace OpenAINET.Chat;

public class ChatResponseUsage
{
    public int PromptTokens { get; set; }
    public int ResponseTokens { get; set; }
    public decimal PromptCost { get; set; }
    public decimal ResponseCost { get; set; }
}

public class ChatConversation
{
    public ChatAPI ChatAPI { get; protected set; }

    public OpenAIModelType ModelType { get; protected set; }
    public List<ChatMessage> Messages { get; set; } = new();
    public int? TotalInputTokens { get; set; }
    public int? TotalOutputTokens { get; set; }
    public int? TotalTokens { get; set; }
    public decimal TotalEstimatedCost { get; set; }
    
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

    public int GetTotalPromptTokens()
    {
        var model = GetModel();
        var encoding = SharpToken.GptEncoding.GetEncodingForModel(model.ModelString);

        var tokens = 0;

        foreach (var message in Messages)
        {
            tokens += message.GetTokenCount(encoding);
            tokens += model.TokensPerMessage ?? 0;

            if (!string.IsNullOrEmpty(message.Name))
            {
                if (model.NameTokensMultiplier.HasValue)
                {
                    var nameTokens = encoding.Encode(message.Name).Count;
                    tokens += (int)(nameTokens * model.NameTokensMultiplier.Value);
                }

                if (model.TokensPerName.HasValue)
                    tokens += model.TokensPerName.Value;
            }
        }

        tokens += model.ResponseTokensPadding ?? 0;

        return tokens;
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

    public async Task<(ChatMessage, ChatResponseUsage)> GetNextAssistantMessageAsync(
        int? maxTokens = null,
        TimeSpan? timeout = null)
    {
        var request = CreateAPIRequest(maxTokens);
        var jsonFormatting = Formatting.None;

        ChatMessage message = null;
        ChatResponseUsage usage = null;

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

                TotalInputTokens += response.usage.prompt_tokens;
                TotalOutputTokens += response.usage.completion_tokens;

                TotalEstimatedCost += promptCost + responseCost;
                TotalTokens = response.usage.total_tokens;
                
                usage = new ChatResponseUsage
                {
                    PromptTokens = response.usage.prompt_tokens,
                    ResponseTokens = response.usage.completion_tokens,
                    PromptCost = promptCost,
                    ResponseCost = responseCost,
                };
            }
        }

        return (message, usage);
    }
}
