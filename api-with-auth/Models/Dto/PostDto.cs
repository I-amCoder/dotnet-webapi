namespace api_with_auth.Models.Dto
{
    public class PostDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public ICollection<CommentDto> Comments { get; set; } = new List<CommentDto>();
        public DateTime CreatedAt { get; set; }
    }
}
