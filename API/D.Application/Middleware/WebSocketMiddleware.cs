using SchoolManagementApi.Websocket;

namespace SchoolManagementApi.Middleware
{
    public class WebSocketMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebSocketHandler _webSocketHandler;

        public WebSocketMiddleware(RequestDelegate next, IWebSocketHandler webSocketHandler)
        {
            _next = next;
            _webSocketHandler = webSocketHandler;
        }

        public async Task Invoke(HttpContext context)
        {
            await _webSocketHandler.HandleWebSocket(context);
        }
    }
}
