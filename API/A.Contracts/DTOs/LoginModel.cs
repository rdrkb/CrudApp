using System.ComponentModel.DataAnnotations;

namespace Contracts.DTOs
{
    public class LoginModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
