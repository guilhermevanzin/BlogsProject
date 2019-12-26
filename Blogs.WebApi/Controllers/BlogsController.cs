using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blogs.WebApi.InputModels;
using Blogs.WebApi.Interfaces;
using Blogs.WebApi.Models;
using Blogs.WebApi.Repository;
using Blogs.WebApi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blogs.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly BloggingContext _bloggingContext;
        private readonly IBlogRepository BlogRepository;
        private readonly IPostRepository PostRepository;

        public BlogsController(DbContextOptions<BloggingContext> options, IBlogRepository blogRepository = null,IPostRepository postRepository = null)
        {    
            _bloggingContext = new BloggingContext(options);    
            
            if(blogRepository != null)
            {
                BlogRepository = blogRepository;
            }
            else
            {
                BlogRepository = new BlogRepository(_bloggingContext);   
            }

            if(postRepository != null)
            {
                PostRepository = postRepository;
            }
            else
            {
                PostRepository = new PostRepository(_bloggingContext);   
            }
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<Blog>>> GetAsync()
        {
            var blogs = await BlogRepository.GetAsync();

            if (!blogs.Any())
                return NoContent();        
    
            return Ok(blogs.Select(b => b));
        }
    
        [HttpGet("{id}")]
        public virtual async Task<ActionResult<Blog>> GetAsync(int id)
        {
            var blog = await BlogRepository.GetAsync(id);

            if (blog == null)
                return NotFound();

            return Ok(blog);
        }
    
        [HttpPost]
        public virtual async Task<ActionResult<BlogViewModel>> PostAsync([FromBody] BlogInputModel inputModel)
        {
            if(inputModel == null)
                return BadRequest();

            await BlogRepository.PostAsync( (Blog) inputModel);   
            
            return Created($"api/blogs/{inputModel.Id}", (BlogViewModel)inputModel);        
        }

        [HttpPut("{id}")]
        public virtual async Task<ActionResult> PutAsync(int id, [FromBody] BlogUpdateInputModel inputModel)
        {
            if(string.IsNullOrWhiteSpace(inputModel.Url) == true)
                return BadRequest();

            var blog = await BlogRepository.GetAsync(id);

            if(blog == null)
                return NotFound();
            
            blog.SetUrl(inputModel.Url);
            await BlogRepository.PutAsync(blog);      

            return Ok();
        }
    
        [HttpDelete("{id}")]
        public virtual async Task<ActionResult> DeleteAsync(int id)
        {
            var blog = await BlogRepository.GetAsync(id);
            
            if(blog==null)
                return NotFound();
        
            await BlogRepository.DeleteAsync(blog);
            
            return Ok();
        }

        [HttpGet("{id}/Posts")]
        public virtual async Task<ActionResult<IEnumerable<PostViewModel>>>GetPostAsync(int id)
        {
            var blog = await BlogRepository.GetAsync(id);

            if(blog == null)
                return NotFound();
            
            var posts = await PostRepository.GetAsync(blog);
            
            if (!posts.Any())
                return NoContent();

            return Ok(posts.Select(p => (PostViewModel) p));
        }
        
        [HttpPost("{id}/Posts")]
        public virtual async Task<ActionResult<PostViewModel>> PostAsync(int id,[FromBody] PostInputModel inputModel)
        {
            var blog =  await BlogRepository.GetAsync(id);
            
            if(blog == null)
                return NotFound();
            
            var post  = new Post(inputModel.Id,inputModel.Title,inputModel.Content,blog); 

            await PostRepository.PostAsync(post);

            return Created($"api/Blogs/{post.Id}/Posts", (PostViewModel) post);
        }

        [HttpPut("{id}/Posts/{idPost}")]
        public virtual async Task<ActionResult> PutAsync(int id, int idPost,[FromBody] PostUpdateInputModel inputModel)
        {
            var blogId = (uint)id;

            var blog = await BlogRepository.GetAsync(id);
            
            if(blog == null)
                return NotFound();

            var post = await PostRepository.GetAsync(idPost);
            
            if(post == null)
                return NotFound();          
            
            if(inputModel.BlogId != 0)
                blogId = inputModel.BlogId;
            
            post.Update(blogId,inputModel.Content,inputModel.Title);
            await PostRepository.PutAsync(post);

            return Ok();
        }

        [HttpDelete("{id}/Posts/{idPost}")]
        public virtual async Task<ActionResult> DeleteAsync(int id, int idPost)
        {
            var blog = await BlogRepository.GetAsync(id);
            if (blog == null)
                return NotFound();

            var post = await PostRepository.GetAsync(idPost);
            
            if(post == null)
                return NotFound();    
            
            await PostRepository.DeleteAsync(post);
        
            return Ok();
        } 
    }
}