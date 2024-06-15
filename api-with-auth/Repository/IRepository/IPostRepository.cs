using api_with_auth.Models;
using System.Linq.Expressions;

namespace api_with_auth.Repository.IRepository
{
    public interface IPostRepository: IBaseRepository<Post> 
    {
        Task<Post> GetPostWithCommentsAsync(Expression<Func<Post,bool>> filter = null);
    }
}
