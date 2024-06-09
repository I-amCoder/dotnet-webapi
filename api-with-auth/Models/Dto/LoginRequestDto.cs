using System.ComponentModel.DataAnnotations;

namespace api_with_auth.Models.Dto
{
    public class LoginRequestDto
    {
        [Required]
        [MaxLength(255)]
        public string Username { get; set;  }

        [Required]
        public string Password { get; set; }
    }
}
