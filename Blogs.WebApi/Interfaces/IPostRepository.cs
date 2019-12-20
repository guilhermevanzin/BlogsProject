using System.Collections.Generic;
using System.Threading.Tasks;
using Blogs.WebApi.Models;

namespace Blogs.WebApi.Interfaces
{
    interface IPostRepository
    {
        Task<IEnumerable<Post>> GetAsync(Blog blog);
        Task<Post> GetAsync(int id);
        Task PostAsync(Post post);
        Task PutAsync(Post post);
        Task DeleteAsync(Post post);   
    }
}