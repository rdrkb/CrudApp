

using System.ComponentModel.DataAnnotations;

namespace A.Contracts.DTOs
{
    public class RegisterModel : LoginModel
    {
        [Required]
        public string Role { get; set; }
    }
}
