using NotificationApi.Websocket.Notification;

namespace NotificationApi.Middleware
{
    public class WSNotificationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWSNotificationHandler _webSocketNotificationHandler;

        public WSNotificationMiddleware(RequestDelegate next, IWSNotificationHandler webSocketNotificationHandler)
        {
            _next = next;
            _webSocketNotificationHandler = webSocketNotificationHandler;
        }

        public async Task Invoke(HttpContext context)
        {
            await _webSocketNotificationHandler.HandleWebSocketForNotification(context);
        }
    }
}
