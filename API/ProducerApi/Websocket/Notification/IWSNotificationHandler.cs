namespace NotificationApi.Websocket.Notification
{
    public interface IWSNotificationHandler
    {
        Task HandleWebSocketForNotification(HttpContext context);
    }
}
