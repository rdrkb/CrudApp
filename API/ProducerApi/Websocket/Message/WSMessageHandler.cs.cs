using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using Business.Security;
using Contracts.DTOs;

namespace NotificationApi.Websocket.Message
{
    public class WSMessageHandler : IWSMessageHandler
    {
        private static ConcurrentDictionary<string, List<WebSocket>> _clients = new ConcurrentDictionary<string, List<WebSocket>>();

        private readonly ITokenService _tokenService;

        public WSMessageHandler(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public async Task HandleWebSocketForMessage(HttpContext context)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                var webSocket = await context.WebSockets.AcceptWebSocketAsync();

                var token = context.Request.Query["token"];

                var username = _tokenService.GetUsernameFromToken(token);

                if (!_clients.ContainsKey(username))
                {
                    _clients[username] = new List<WebSocket>();
                }

                _clients[username].Add(webSocket);

                await Receive(webSocket);

                _clients[username].Remove(webSocket);

                if (_clients[username].Count == 0)
                {
                    _clients.TryRemove(username, out _);
                }
            }
            else
            {
                context.Response.StatusCode = 400;
            }
        }
        private async Task Receive(WebSocket webSocket)
        {
            var buffer = new byte[1024];

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
            if (_clients.TryGetValue(message.Receiver, out List<WebSocket> receiverWebSockets))
            {
                // If the receiver is connected, send the message to all WebSocket instances associated with the receiver username
                var jsonMessage = JsonConvert.SerializeObject(message, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
                var buffer = Encoding.UTF8.GetBytes(jsonMessage);
                foreach (var receiverWebSocket in receiverWebSockets)
                {
                    await receiverWebSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }

            // Also send the message to all WebSocket instances associated with the sender username
            if (_clients.TryGetValue(message.Sender, out List<WebSocket> senderWebSockets))
            {
                var jsonMessage = JsonConvert.SerializeObject(message, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
                var buffer = Encoding.UTF8.GetBytes(jsonMessage);
                foreach (var senderWebSocket in senderWebSockets)
                {
                    await senderWebSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }
    }
}
