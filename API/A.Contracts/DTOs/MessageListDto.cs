namespace Contracts.DTOs
{
    public class MessageListDto
    {
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public DateTime LastMessage { get; set; }
    }
}
