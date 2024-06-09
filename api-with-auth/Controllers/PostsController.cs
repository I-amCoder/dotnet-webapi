
using Microsoft.AspNetCore.Mvc;
using api_with_auth.Models;
using api_with_auth.Data;
using Microsoft.EntityFrameworkCore;
using api_with_auth.Models.Dto;
using Microsoft.AspNetCore.Authorization;

namespace api_with_auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        
        public PostsController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostDto>>> GetPosts()
        {
            var posts = await _dbContext.Posts
              .Select(post => new PostDto{ 
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                CreatedAt = post.CreatedAt,
                Comments = post.Comments.Select(c=> new CommentDto
                {
                    Id = c.Id,
                    PostId = c.Id,
                    CreatedAt = c.CreatedAt,
                    Content = c.Content
                }).ToList()
            }).ToListAsync();

            return Ok(posts);
        }

        [HttpGet("{Id:int}")]
        public async Task<ActionResult<Post>> GetPost(int Id)
        {
            var post = await _dbContext
                .Posts.Where(p=>p.Id == Id)
                .Select(p=> new PostDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Content = p.Content,
                    CreatedAt = p.CreatedAt,
                    Comments = p.Comments.Select(c=> new CommentDto
                    {
                        Id=c.Id,
                        Content=c.Content,  
                        CreatedAt=c.CreatedAt,
                        PostId  = p.Id
                    }).ToList()
                }).FirstOrDefaultAsync();
                
            
            if (post == null)
            {
                return NotFound("Post Not Found");
            }


         
            return Ok(post);
        }

        [HttpPost]
        [Authorize()]
        public async Task<ActionResult<PostDto>> CreatePost(CreatePostDto createDto)
        {
            // Map properties manually
            Post post = new()
            {
                Title = createDto.Title,
                Content = createDto.Content,
                CreatedAt = DateTime.Now,
            };

            _dbContext.Posts.Add(post);
            await _dbContext.SaveChangesAsync();

            // New Instance of PostDto
            return CreatedAtAction(nameof(GetPost), new { post.Id}, post);
        }
    }
}
