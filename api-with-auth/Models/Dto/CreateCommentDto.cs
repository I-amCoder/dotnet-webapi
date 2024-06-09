using System.ComponentModel.DataAnnotations;

namespace api_with_auth.Models.Dto
{
    public class CreateCommentDto
    {
        [Required]
        [MaxLength(255)]
        public string Content { get; set; }
    }
}
