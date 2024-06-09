using api_with_auth.Models;

namespace api_with_auth.Repository.IRepository
{
    public interface IPostRepository: IBaseRepository<Post> 
    {
        Task<Post> GetPostWithCommentsAsync(int id);
    }
}
