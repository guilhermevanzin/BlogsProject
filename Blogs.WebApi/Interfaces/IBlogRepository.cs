using System.Collections.Generic;
using System.Threading.Tasks;
using Blogs.WebApi.Models;

namespace Blogs.WebApi.Interfaces
{
    public interface IBlogRepository
    {
        Task<IEnumerable<Blog>> GetAsync();
        Task<Blog> GetAsync(int id);
        Task PostAsync(Blog blog);
        Task PutAsync(Blog blog);
        Task DeleteAsync(Blog blog);
    }
}