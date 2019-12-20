using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blogs.WebApi.Interfaces;
using Blogs.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Blogs.WebApi.Repository
{
    public class BlogRepository: IBlogRepository
    {
        private readonly BloggingContext _bloggingContext;

        public BlogRepository(BloggingContext _context)
        {
            _bloggingContext = _context;
        }
        public BlogRepository()
        {
        }

        public virtual Task<IEnumerable<Blog>> GetAsync()
        {
            return GetAllAsync();
        }
        
        public virtual Task<Blog> GetAsync(int id)
        {
            return GetByIdAsync((uint)id);
        }

        public virtual async Task PostAsync(Blog blog)
        {
            _bloggingContext.Add(blog);
            await _bloggingContext.SaveChangesAsync();
        }

        public virtual async Task PutAsync(Blog blog)
        {
            _bloggingContext.Blogs.Update(blog);
            await _bloggingContext.SaveChangesAsync();
        }
        public virtual async Task DeleteAsync(Blog blog)
        {
            _bloggingContext.Blogs.Remove(blog);
            await _bloggingContext.SaveChangesAsync();
        }

        public virtual async Task<Blog> GetAsync(uint id)
        {
            return await GetByIdAsync(id);
        }
        
        public virtual async Task<IEnumerable<Blog>> GetAllAsync()
        {
            var blogs = await _bloggingContext.Blogs.ToListAsync();    
            return blogs.Select(b => b).OrderBy(b => b.Id);
        }
        
        public virtual  Task<Blog> GetByIdAsync(uint id)
            => _bloggingContext.Blogs.SingleOrDefaultAsync(b => b.Id == id);
    }
}
