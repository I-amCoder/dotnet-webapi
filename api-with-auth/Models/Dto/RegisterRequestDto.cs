using System.ComponentModel.DataAnnotations;

namespace api_with_auth.Models.Dto
{
    public class RegisterRequestDto
    {
        [Required]
        public string UserName {  get; set; }
        
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email {  get; set; }

        [Required]
        public string Password { get; set; }
    }
}
