using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A.Contracts.DTOs
{
    public class MessageListDto
    {
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public DateTime LastMessage { get; set; }
    }
}
