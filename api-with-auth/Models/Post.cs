﻿namespace api_with_auth.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}
