
using Business.Security;
using Contracts.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NotificationApi.Contracts.Models;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;

namespace NotificationApi.Websocket.Notification
{
    public class WSNotificationHandler : IWSNotificationHandler
    {
        private static ConcurrentDictionary<string, List<WebSocket>> _clients = new ConcurrentDictionary<string, List<WebSocket>>();
        private readonly ITokenService _tokenService;

        public WSNotificationHandler(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public async Task HandleWebSocketForNotification(HttpContext context)
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
                while(webSocket.State == WebSocketState.Open)
                {
                    var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                    if(result.MessageType == WebSocketMessageType.Text)
                    {
                        var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        var notificationModel = JsonConvert.DeserializeObject<UserNotification>(message);

                        await SendNotification(notificationModel);
                    }
                    else if(result.MessageType == WebSocketMessageType.Close)
                    {
                        break;
                    }
                }
            }
            catch (WebSocketException)
            {
                // connection closed by the client
            }
            finally
            {
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Connection closed", CancellationToken.None);
            }
        }

        private async Task SendNotification(UserNotification? notificationModel)
        {
            if (_clients.TryGetValue(notificationModel.Receiver, out List<WebSocket> receiverWebSockets))
            {
                var jsonMessage = JsonConvert.SerializeObject(notificationModel, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
                var buffer = Encoding.UTF8.GetBytes(jsonMessage);
                foreach (var receiverWebSocket in receiverWebSockets)
                {
                    await receiverWebSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }
    }
}
