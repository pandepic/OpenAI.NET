using System;
using System.Collections.Generic;
using System.Text;

namespace OpenAINET
{
    public class APIError
    {
        public string message { get; set; }
        public string type { get; set; }
        public string param { get; set; }
        public string code { get; set; }
    }
}
