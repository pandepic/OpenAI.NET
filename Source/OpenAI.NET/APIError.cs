using Newtonsoft.Json;

namespace OpenAINET;

public class APIError
{
    public string message { get; set; }
    public string type { get; set; }
    public string param { get; set; }
    public string code { get; set; }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}
