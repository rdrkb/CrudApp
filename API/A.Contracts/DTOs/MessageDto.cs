
using System.ComponentModel.DataAnnotations;

namespace A.Contracts.DTOs
{
    public class MessageDto
    {
        [Required]
        public string Sender { get; set; }
        [Required]
        public string Receiver { get; set; }
        [Required]
        public string Content { get; set; }

    }
}
