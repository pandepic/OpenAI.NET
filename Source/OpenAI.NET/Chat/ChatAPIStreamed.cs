//using System;
//using System.IO;
//using System.Net.Http;
//using System.Net.Http.Headers;
//using System.Net.Http.Json;
//using System.Text;
//using System.Text.Json;
//using System.Text.Json.Serialization;
//using System.Threading;
//using System.Threading.Tasks;
//using OpenAINET.Chat.DTO;

//namespace OpenAINET.Chat
//{
//    public class ChatAPIStreamed : IDisposable
//    {
//        protected HttpClient _httpClient;
//        protected StringBuilder _responseString = new StringBuilder();

//        public readonly string APIKey;
//        public readonly ChatConversation Conversation;

//        public event Action<string> OnTokenReceived;
//        public event Action<APIError> OnError;
//        public event Action<ChatMessage> OnMessageComplete;
//        public event Action<Exception> OnException;

//        public bool IsStreamingResponse { get; protected set; }

//        public ChatAPIStreamed(string apiKey, ChatConversation conversation)
//        {
//            APIKey = apiKey;
//            Conversation = conversation;

//            _httpClient = new HttpClient();
//            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
//        }

//        public void Dispose()
//        {
//            _httpClient?.Dispose();
//            _httpClient = null;
//        }

//        public void WaitForResponse()
//        {
//            while (IsStreamingResponse)
//                Thread.Sleep(10);
//        }

//        public void StartStreamingResponse(float? temperature = null)
//        {
//            if (IsStreamingResponse)
//                throw new Exception("Already streaming a response.");

//            if (!ChatAPI.SupportedModels.Contains(Conversation.ModelType))
//                throw new Exception($"Conversation model type not supported by chat API: {Conversation.ModelType}");

//            IsStreamingResponse = true;

//            Task.Run(async () =>
//            {
//                try
//                {
//                    var request = new ChatAPIRequest(Conversation)
//                    {
//                        stream = true,
//                    };

//                    if (temperature.HasValue)
//                        request.temperature = temperature.Value;

//                    var content = JsonContent.Create(request, null, new JsonSerializerOptions
//                    {
//                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
//                    });

//                    using var httpRequest = new HttpRequestMessage(HttpMethod.Post, ChatAPI.API_PATH);
//                    httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/event-stream"));
//                    httpRequest.Content = content;

//                    using var httpResponse = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead);
                    
//                    using var stream = await httpResponse.Content.ReadAsStreamAsync();
//                    using var reader = new StreamReader(stream);

//                    _responseString.Clear();

//                    while (true)
//                    {
//                        if (reader.EndOfStream)
//                            continue;

//                        var line = await reader.ReadLineAsync();

//                        if (string.IsNullOrEmpty(line))
//                            continue;

//                        if (line.StartsWith("data: "))
//                            line = line.Substring("data: ".Length);

//                        if (line.StartsWith("[DONE]"))
//                            break;

//                        try
//                        {
//                            var response = JsonSerializer.Deserialize<ChatAPIResponse>(line);

//                            if (response.choices != null
//                                && response.choices.Count > 0
//                                && response.choices[0].delta != null)
//                            {
//                                var delta = response.choices[0].delta;
//                                var deltaMessage = delta.content;

//                                _responseString.Append(deltaMessage);
//                                OnTokenReceived?.Invoke(deltaMessage);
//                            }
//                        }
//                        catch (Exception)
//                        {
//                            line += await reader.ReadToEndAsync();
//                            var errorResponse = JsonSerializer.Deserialize<ChatAPIResponse>(line);

//                            if (errorResponse.error != null)
//                                OnError?.Invoke(errorResponse.error);

//                            break;
//                        }

//                        Thread.Sleep(10);
//                    }

//                    var responseMessage = ChatMessage.FromAssistant(_responseString.ToString());

//                    Conversation.AddMessage(responseMessage);
//                    OnMessageComplete?.Invoke(responseMessage);
//                }
//                catch (Exception ex)
//                {
//                    OnException?.Invoke(ex);
//                }
//                finally
//                {
//                    IsStreamingResponse = false;
//                }
//            });
//        }
//    }
//}
