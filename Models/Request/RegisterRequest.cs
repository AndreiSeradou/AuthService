using System.ComponentModel.DataAnnotations;

namespace AuthService.Models.Request
{
    public class RegisterRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
