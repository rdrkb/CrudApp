
using A.Contracts.DTOs;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using C.Business.Security;

namespace D.Application.Websocket
{
    public class WebSocketHandler : IWebSocketHandler
    {
        private static ConcurrentDictionary<string, WebSocket> _clients = new ConcurrentDictionary<string, WebSocket>();

        private readonly ITokenService _tokenService;

        public WebSocketHandler(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public async Task HandleWebSocket(HttpContext context)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                var webSocket = await context.WebSockets.AcceptWebSocketAsync();

                var token = context.Request.Query["token"];

                var username = _tokenService.GetUsernameFromToken(token);

                _clients.TryAdd(username, webSocket);

                await Receive(webSocket);

                _clients.TryRemove(username, out _); 
            }
            else
            {
                context.Response.StatusCode = 400;
            }
        }
        private async Task Receive(WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];

            try
            {
                while (webSocket.State == WebSocketState.Open)
                {
                    var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                    if (result.MessageType == WebSocketMessageType.Text)
                    {

                        var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        var messageModel = JsonConvert.DeserializeObject<MessageDto>(message);


                        await SendMessage(messageModel);
                    }
                    else if (result.MessageType == WebSocketMessageType.Close)
                    {
                        break;
                    }
                }
            }
            catch (WebSocketException)
            {
                // Connection closed by the client
            }
            finally
            {
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Connection closed", CancellationToken.None);
            }
        }
        public async Task SendMessage(MessageDto message)
        {
            if (_clients.TryGetValue(message.Receiver, out WebSocket receiverWebSocket))
            {
                // If the receiver is connected, send the message directly to them
                var jsonMessage = JsonConvert.SerializeObject(message, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
                var buffer = Encoding.UTF8.GetBytes(jsonMessage);
                await receiverWebSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            }

            // Also send the message to the sender
            if (_clients.TryGetValue(message.Sender, out WebSocket senderWebSocket))
            {
                var jsonMessage = JsonConvert.SerializeObject(message, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
                var buffer = Encoding.UTF8.GetBytes(jsonMessage);
                await senderWebSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            }

        }
    }
}
