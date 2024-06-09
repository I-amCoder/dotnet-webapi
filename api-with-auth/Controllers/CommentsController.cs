using api_with_auth.Data;
using api_with_auth.Models;
using api_with_auth.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace api_with_auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public CommentsController(AppDbContext dbContext) {  _dbContext = dbContext; }

        [HttpPost("{postId:int}")]
        public async Task<ActionResult<CommentDto>> SaveComment(int postId,CreateCommentDto createDto)
        {
            var post = await _dbContext.Posts.FindAsync(postId);
            if(post == null)
            {
                return NotFound("Comment Not Found");
            }

            Comment comment = new()
            {
                PostId = postId,
                Content = createDto.Content,
                CreatedAt = DateTime.Now
            };

            await _dbContext.Comments.AddAsync(comment);
            await _dbContext.SaveChangesAsync();

            CommentDto commentDto = new()
            {
                Id = comment.Id,
                PostId = postId,
                Content = comment.Content,
                CreatedAt = DateTime.Now
            };


            return Ok(commentDto);
        }
    }
}
