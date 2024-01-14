using System.Collections.Generic;

namespace OpenAINET.Moderation.DTO;

public class ModerationAPIResults
{
    public bool flagged { get; set; }
    public Dictionary<string, bool> categories { get; set; }
    public Dictionary<string, float> category_scores { get; set; }
}

public class ModerationAPIResponse
{
    public string id { get; set; }
    public string model { get; set; }
    public List<ModerationAPIResults> results { get; set; } 
}