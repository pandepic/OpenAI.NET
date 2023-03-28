using System.Collections.Generic;

namespace OpenAINET.Text.DTO
{
    public class TextAPIResponseChoiceLogProbs
    {
        public List<string> tokens { get; set; }
        public List<double> token_logprobs { get; set; }
        public List<Dictionary<string, double>> top_logprobs { get; set; }
        public List<int> text_offset { get; set; }
    }

    public class TextAPIResponseChoice
    {
        public string text { get; set; }
        public int? index { get; set; }
        public string finish_reason { get; set; }
        public TextAPIResponseChoiceLogProbs logprobs { get; set; }
    }

    public class TextAPIResponseUsage
    {
        public int prompt_tokens { get; set; }
        public int completion_tokens { get; set; }
        public int total_tokens { get; set; }
    }

    public class TextAPIResponse
    {
        public APIError error { get; set; }
        public string model { get; set; }
        public string id { get; set; }
        public List<TextAPIResponseChoice> choices { get; set; }
        public TextAPIResponseUsage usage { get; set; }
        public string @object { get; set; }
        public long created { get; set; }
    }
}
