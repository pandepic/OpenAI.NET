using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using OpenAINET.Chat.DTO;

namespace OpenAINET.Chat
{
    public class ChatAPIStreamed : IDisposable
    {
        protected HttpClient _httpClient;

        public readonly string APIKey;
        public readonly ChatConversation Conversation;

        public event Action<string> OnTokenReceived;
        public event Action<APIError> OnError;
        public event Action<ChatMessage> OnMessageComplete;
        public event Action<Exception> OnException;

        public bool IsStreamingResponse { get; protected set; }

        public ChatAPIStreamed(string apiKey, ChatConversation conversation)
        {
            APIKey = apiKey;
            Conversation = conversation;

            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
            _httpClient = null;
        }

        public void StartStreamingResponse()
        {
            if (IsStreamingResponse)
                throw new Exception("Already streaming a response.");

            if (!ChatAPI.SupportedModels.Contains(Conversation.ModelType))
                throw new Exception($"Conversation model type not supported by chat API: {Conversation.ModelType}");

            IsStreamingResponse = true;

            Task.Run(async () =>
            {
                try
                {
                    var request = new ChatAPIRequest(Conversation)
                    {
                        stream = true,
                    };

                    var content = JsonContent.Create(request, null, new JsonSerializerOptions
                    {
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
                    });

                    using var httpRequest = new HttpRequestMessage(HttpMethod.Post, ChatAPI.API_PATH);
                    httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/event-stream"));
                    httpRequest.Content = content;

                    var httpResponse = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead);

                    using var stream = await httpResponse.Content.ReadAsStreamAsync();
                    using var reader = new StreamReader(stream);

                    var responseMessage = ChatMessage.FromAssistant("");

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
                            var response = JsonSerializer.Deserialize<ChatAPIResponse>(line);

                            if (response.choices != null
                                && response.choices.Count > 0
                                && response.choices[0].delta != null)
                            {
                                var delta = response.choices[0].delta;
                                var deltaMessage = delta.content;
                                responseMessage.Message += deltaMessage;

                                OnTokenReceived?.Invoke(deltaMessage);
                            }
                        }
                        catch (Exception ex)
                        {
                            line += await reader.ReadToEndAsync();
                            var errorResponse = JsonSerializer.Deserialize<ChatAPIResponse>(line);

                            if (errorResponse.error != null && !string.IsNullOrEmpty(errorResponse.error.message))
                                OnError?.Invoke(errorResponse.error);

                            break;
                        }

                        Thread.Sleep(10);
                    }

                    Conversation.AddMessage(responseMessage);
                    OnMessageComplete?.Invoke(responseMessage);
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
