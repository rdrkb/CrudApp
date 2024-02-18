using A.Contracts.DTOs;
using C.Business.TokenServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;

namespace D.Application.Controllers
{
    public class WebSocketController : ControllerBase
    {
        private static ConcurrentDictionary<string, WebSocket> _clients = new ConcurrentDictionary<string, WebSocket>();

        private readonly ILogger<WebSocketController> _logger;
        private readonly ITokenService _tokenService;

        public WebSocketController(ILogger<WebSocketController> logger, ITokenService tokenService)
        {
            _logger = logger;
            _tokenService = tokenService;
        }

        private void LogClientsInformation()
        {
            _logger.LogInformation("Clients information:");
            foreach (var client in _clients)
            {
                _logger.LogInformation($"ClientId: {client.Key}, WebSocket: {client.Value}");
            }
        }

        [Route("/ws")]
        [HttpGet]
        public async Task Get()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();

                var token = HttpContext.Request.Query["token"];


                var username = _tokenService.GetUsernameFromToken(token);

                //Console.WriteLine($"WebSocket connection established for username: {username}");

                _clients.TryAdd(username, webSocket); // Add the new client to the dictionary

                //LogClientsInformation();
                
                await Receive(webSocket); // Start listening for messages from this client

                _clients.TryRemove(username, out _); // Remove the client when the connection is closed
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
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

            // Add handling for offline or invalid receiver scenarios if needed
        }
    }
}
