using System.Collections.Generic;

namespace OpenAINET.Text.DTO
{
    public class TextAPIRequest
    {
        public string model { get; set; }
        public string[] prompt { get; set; }
        public string suffix { get; set; }
        public int max_tokens { get; set; } = 16;
        public float temperature { get; set; } = 1f;
        public float top_p { get; set; } = 1f;
        public int n { get; set; } = 1;
        public bool stream { get; set; }
        public int? logprobs { get; set; }
        public bool echo { get; set; }
        public string[] stop { get; set; }
        public float presence_penalty { get; set; }
        public float frequency_penalty { get; set; }
        public int best_of { get; set; } = 1;
        //public object logit_bias { get; set; } = null;
        //public string user { get; set; }
    }
}
