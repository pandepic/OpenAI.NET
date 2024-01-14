using System.Collections.Generic;

namespace OpenAINET.Embeddings.DTO;

public class EmbeddingResponseData
{
    public int index { get; set; }
    public List<float> embedding { get; set; }
    public string @object { get; set; }
}

public class EmbeddingsAPIResponseUsage
{
    public int prompt_tokens { get; set; }
    public int completion_tokens { get; set; }
    public int total_tokens { get; set; }
}

public class EmbeddingsAPIResponse
{
    public string @object { get; set; }
    public List<EmbeddingResponseData> data { get; set; }
    public string model { get; set; }
    public EmbeddingsAPIResponseUsage usage { get; set; }
    public APIError error { get; set; }
}