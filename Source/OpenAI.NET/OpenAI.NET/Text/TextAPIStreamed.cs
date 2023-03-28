using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using OpenAINET.Chat;
using OpenAINET.Chat.DTO;
using OpenAINET.Text.DTO;
using System.Threading;
using System.Text;

namespace OpenAINET.Text
{
    public class TextAPIStreamed : IDisposable
    {
        protected HttpClient _httpClient;
        protected StringBuilder _responseString = new StringBuilder();

        public readonly OpenAIModel Model;
        public readonly string APIKey;

        public event Action<string> OnTokenReceived;
        public event Action<APIError> OnError;
        public event Action<string> OnResponseComplete;
        public event Action<Exception> OnException;

        public bool IsStreamingResponse { get; protected set; }

        public TextAPIStreamed(OpenAIModelType modelType, string apiKey)
        {
            Model = OpenAIModel.Models[modelType];
            APIKey = apiKey;

            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
            _httpClient = null;
        }

        public void StartStreamingResponse(string prompt, float temperature = 1f, int? maxTokens = null)
        {
            StartStreamingResponse(new string[] { prompt }, temperature, maxTokens);
        }

        public void StartStreamingResponse(string[] prompt, float temperature = 1f, int? maxTokens = null)
        {
            var useMaxTokens = maxTokens ?? Model.MaxTokens;

            var request = new TextAPIRequest()
            {
                model = Model.ModelString,
                prompt = prompt,
                max_tokens = useMaxTokens,
                temperature = temperature,
                stream = true,
            };

            StartStreamingResponse(request);
        }

        public void WaitForResponse()
        {
            while (IsStreamingResponse)
                Thread.Sleep(10);
        }

        public void StartStreamingResponse(TextAPIRequest request)
        {
            if (IsStreamingResponse)
                throw new Exception("Already streaming a response.");

            IsStreamingResponse = true;

            Task.Run(async () =>
            {
                try
                {
                    request.stream = true;

                    var content = JsonContent.Create(request, null, new JsonSerializerOptions
                    {
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
                    });

                    using var httpRequest = new HttpRequestMessage(HttpMethod.Post, TextAPI.API_PATH);
                    httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/event-stream"));
                    httpRequest.Content = content;

                    using var httpResponse = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead);

                    using var stream = await httpResponse.Content.ReadAsStreamAsync();
                    using var reader = new StreamReader(stream);

                    _responseString.Clear();

                    while (true)
                    {
                        if (reader.EndOfStream)
                            continue;

                        var line = await reader.ReadLineAsync();

                        if (string.IsNullOrEmpty(line))
                            continue;

                        if (line.StartsWith("data: "))
                            line = line.Substring("data: ".Length);

                        if (line.StartsWith("[DONE]"))
                            break;

                        try
                        {
                            var response = JsonSerializer.Deserialize<TextAPIResponse>(line);

                            if (response.choices != null
                                && response.choices.Count > 0)
                            {
                                foreach (var choice in response.choices)
                                {
                                    _responseString.Append(choice.text);
                                    OnTokenReceived?.Invoke(choice.text);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            line += await reader.ReadToEndAsync();
                            var errorResponse = JsonSerializer.Deserialize<ChatAPIResponse>(line);

                            if (errorResponse.error != null)
                                OnError?.Invoke(errorResponse.error);

                            break;
                        }

                        Thread.Sleep(10);
                    }

                    OnResponseComplete?.Invoke(_responseString.ToString());
                }
                catch (Exception ex)
                {
                    OnException?.Invoke(ex);
                }
                finally
                {
                    IsStreamingResponse = false;
                }
            });
        }
    }
}
