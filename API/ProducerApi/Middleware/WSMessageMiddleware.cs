using NotificationApi.Websocket.Message;

namespace NotificationApi.Middleware
{
    public class WSMessageMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWSMessageHandler _webSocketMessageHandler;

        public WSMessageMiddleware(RequestDelegate next, IWSMessageHandler webSocketMessageHandler)
        {
            _next = next;
            _webSocketMessageHandler = webSocketMessageHandler;
        }

        public async Task Invoke(HttpContext context)
        {
            await _webSocketMessageHandler.HandleWebSocketForMessage(context);
        }
    }
}
