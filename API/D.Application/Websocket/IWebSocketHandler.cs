namespace D.Application.Websocket
{
    public interface IWebSocketHandler
    {
        Task HandleWebSocket(HttpContext context);
    }
}
