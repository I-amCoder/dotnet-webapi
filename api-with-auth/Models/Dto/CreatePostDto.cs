using System.ComponentModel.DataAnnotations;

namespace api_with_auth.Models.Dto
{
    public class CreatePostDto
    {
        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
