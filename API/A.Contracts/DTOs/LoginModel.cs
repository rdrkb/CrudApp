

using System.ComponentModel.DataAnnotations;

namespace A.Contracts.DTOs
{
    public class LoginModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
