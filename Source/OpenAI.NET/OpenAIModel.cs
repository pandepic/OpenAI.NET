using System;
using System.Collections.Generic;
using System.Text;

namespace OpenAINET
{
    public enum OpenAIModelType
    {
        GPT3_5Turbo,
        GPT3_5Turbo16k,
        TextDavinci003,
        GPT4_8k,
        GPT4_32k,
        TextEmbeddingAda002,
    }

    public class OpenAIModel
    {
        public OpenAIModelType ModelType;
        public string ModelString;
        public int MaxTokens;
        public decimal InputPricePer1kTokens;
        public decimal OutputPricePer1kTokens;

        public static readonly Dictionary<OpenAIModelType, OpenAIModel> Models = new Dictionary<OpenAIModelType, OpenAIModel>()
        {
            {
                OpenAIModelType.GPT3_5Turbo,
                new OpenAIModel()
                {
                    ModelType = OpenAIModelType.GPT3_5Turbo,
                    ModelString = "gpt-3.5-turbo",
                    MaxTokens = 4096,
                    InputPricePer1kTokens = 0.0015m,
                    OutputPricePer1kTokens = 0.002m,
                }
            },
            {
                OpenAIModelType.GPT3_5Turbo16k,
                new OpenAIModel()
                {
                    ModelType = OpenAIModelType.GPT3_5Turbo16k,
                    ModelString = "gpt-3.5-turbo-16k",
                    MaxTokens = 16384,
                    InputPricePer1kTokens = 0.003m,
                    OutputPricePer1kTokens = 0.004m,
                }
            },
            {
                OpenAIModelType.TextDavinci003,
                new OpenAIModel()
                {
                    ModelType = OpenAIModelType.TextDavinci003,
                    ModelString = "text-davinci-003",
                    MaxTokens = 4096,
                    InputPricePer1kTokens = 0.02m,
                    OutputPricePer1kTokens = 0.02m,
                }
            },
            {
                OpenAIModelType.GPT4_8k,
                new OpenAIModel()
                {
                    ModelType = OpenAIModelType.GPT4_8k,
                    ModelString = "gpt-4",
                    MaxTokens = 8192,
                    InputPricePer1kTokens = 0.03m,
                    OutputPricePer1kTokens = 0.06m,
                }
            },
            {
                OpenAIModelType.GPT4_32k,
                new OpenAIModel()
                {
                    ModelType = OpenAIModelType.GPT4_32k,
                    ModelString = "gpt-4",
                    MaxTokens = 32768,
                    InputPricePer1kTokens = 0.06m,
                    OutputPricePer1kTokens = 0.12m,
                }
            },
            {
                OpenAIModelType.TextEmbeddingAda002,
                new OpenAIModel()
                {
                    ModelType = OpenAIModelType.TextEmbeddingAda002,
                    ModelString = "text-embedding-ada-002",
                    MaxTokens = 32768,
                    InputPricePer1kTokens = 0.0001m,
                    OutputPricePer1kTokens = 0.0001m,
                }
            }
        };
    }
}
