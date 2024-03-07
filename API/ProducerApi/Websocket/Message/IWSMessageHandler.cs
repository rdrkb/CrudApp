namespace NotificationApi.Websocket.Message
{
    public interface IWSMessageHandler
    {
        Task HandleWebSocketForMessage(HttpContext context);
    }
}
