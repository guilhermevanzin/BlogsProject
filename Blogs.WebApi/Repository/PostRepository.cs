using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blogs.WebApi.Interfaces;
using Blogs.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Blogs.WebApi.Repository
{
    public class PostRepository : IPostRepository 
    {
        private readonly BloggingContext _bloggingContext;

        public PostRepository(BloggingContext _context)
        {
            _bloggingContext = _context;
        }

        public PostRepository()
        {
        }

        public virtual async Task<Post> GetAsync(int idPost)
        {
            var posts =  await _bloggingContext.Posts.ToListAsync();
            return posts.FirstOrDefault(p => p.Id == idPost);
        }

        public virtual async Task<IEnumerable<Post>> GetAsync(Blog blog)
        {
            var posts = await _bloggingContext.Posts.ToListAsync();
            return posts.FindAll(p => p.BlogId == blog.Id);     
        }

        public virtual async Task PostAsync(Post post)
        {   
            _bloggingContext.Add(post);
            await _bloggingContext.SaveChangesAsync();   
        }

        public virtual async Task PutAsync(Post post)
        {
            _bloggingContext.Update(post);
            await _bloggingContext.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(Post post)
        {   
            _bloggingContext.Posts.Remove(post);
            await _bloggingContext.SaveChangesAsync();   
        } 
        
        public virtual async Task<Post> GetByIdAsync(Blog blog, int idPost)
        { 
            var blogs = await _bloggingContext.Blogs.ToListAsync();
            var blg =  blogs.SingleOrDefault(b => b.Id == blog.Id);
            var post = await blg.Posts.AsQueryable().ToListAsync();
            var pst = post.SingleOrDefault(p => p.Id == idPost);
            return pst;
        }
    }
}