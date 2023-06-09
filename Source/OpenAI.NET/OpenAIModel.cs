﻿using System;
using System.Collections.Generic;
using System.Text;

namespace OpenAINET
{
    public enum OpenAIModelType
    {
        GPT3_5Turbo,
        TextDavinci003,
        GPT4_8k,
        GPT4_32k,
    }

    public class OpenAIModel
    {
        public OpenAIModelType ModelType;
        public string ModelString;
        public int MaxTokens;

        public static readonly Dictionary<OpenAIModelType, OpenAIModel> Models = new Dictionary<OpenAIModelType, OpenAIModel>()
        {
            {
                OpenAIModelType.GPT3_5Turbo,
                new OpenAIModel()
                {
                    ModelType = OpenAIModelType.GPT3_5Turbo,
                    ModelString = "gpt-3.5-turbo",
                    MaxTokens = 4096,
                }
            },
            {
                OpenAIModelType.TextDavinci003,
                new OpenAIModel()
                {
                    ModelType = OpenAIModelType.TextDavinci003,
                    ModelString = "text-davinci-003",
                    MaxTokens = 4096,
                }
            },
            {
                OpenAIModelType.GPT4_8k,
                new OpenAIModel()
                {
                    ModelType = OpenAIModelType.GPT4_8k,
                    ModelString = "gpt-4",
                    MaxTokens = 8192,
                }
            },
            {
                OpenAIModelType.GPT4_32k,
                new OpenAIModel()
                {
                    ModelType = OpenAIModelType.GPT4_32k,
                    ModelString = "gpt-4",
                    MaxTokens = 32768,
                }
            }
        };
    }
}
