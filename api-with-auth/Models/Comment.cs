namespace api_with_auth.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        public int PostId { get; set; }

        public Post Post { get; set; }
    }
}
