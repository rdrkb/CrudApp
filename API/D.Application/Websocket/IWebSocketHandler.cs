namespace SchoolManagementApi.Websocket
{
    public interface IWebSocketHandler
    {
        Task HandleWebSocket(HttpContext context);
    }
}
