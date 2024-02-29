using System.ComponentModel.DataAnnotations;

namespace Contracts.DTOs
{
    public class RegisterModel : LoginModel
    {
        [Required]
        public string Role { get; set; }
    }
}
