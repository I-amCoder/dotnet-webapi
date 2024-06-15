using api_with_auth.Data;
using api_with_auth.Models;
using api_with_auth.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace api_with_auth.Repository
{
    public class PostRepository : BaseRepository<Post>, IPostRepository
    {
        private readonly AppDbContext _dbContext;

        public PostRepository(AppDbContext dbContext): base(dbContext) 
        {   
            _dbContext = dbContext;
        }

        public async Task<Post> GetPostWithCommentsAsync(Expression<Func<Post, bool>> filter = null)
        {
            IQueryable<Post> query = _dbContext.Set<Post>();
            if(filter !=null)
            {
                query = query.Where(filter);
            }

            return await query.FirstOrDefaultAsync();
            
        }
    }
}
