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
        private IBlogRepository BlogRepository;
        private IPostRepository PostRepository;
        

        public BlogsController(DbContextOptions<BloggingContext> options)
        {    
            _bloggingContext = new BloggingContext(options);
            
            BlogRepository = new BlogRepository(_bloggingContext);
            
            PostRepository = new PostRepository(_bloggingContext);
        }

        public void SetBlogRepository(BlogRepository blogRepository)
        {
            BlogRepository = blogRepository;   
        }
        public void SetPostRepository(PostRepository postRepository)
        {
            PostRepository = postRepository;   
        }


        //GET: api/Blogs
        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<BlogViewModel>>> GetAsync()
        {
            var blogs = await BlogRepository.GetAsync();

            if (!blogs.Any())
                return NoContent();
    
            return Ok(blogs.Select(b => (BlogViewModel) b));
        }
    
        //GET: api/Blogs/id
        [HttpGet("{id}")]
        public virtual async Task<ActionResult<BlogViewModel>> GetAsync(int id)
        {
            var blog = await BlogRepository.GetAsync(id);
            if (blog == null)
                return NotFound();
            return Ok((BlogViewModel) blog);
        }
    
        //POST: api/Blogs
        [HttpPost]
        public virtual async Task<ActionResult<BlogViewModel>> PostAsync([FromBody] BlogInputModel inputModel)
        {
            if(inputModel == null)
                return BadRequest();

            await BlogRepository.PostAsync( (Blog) inputModel);   
            
            return Created($"api/blogs/{inputModel.Id}", (BlogViewModel)inputModel);        
        }

        //PUT: api/Blogs/id
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
    
        //DELETE: api/Blogs
        [HttpDelete("{id}")]
        public virtual async Task<ActionResult> DeleteAsync(int id)
        {
            var blog = await BlogRepository.GetAsync(id);
            
            if(blog==null)
                return NotFound();
        
            await BlogRepository.DeleteAsync(blog);
            
            return Ok();
        }

        //GET: api/Blogs/id/Posts
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
        
        //POST: api/Blogs/id/Posts
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

        //PUT: api/Blogs/id/Posts/idp
        [HttpPut("{id}/Posts/{idPost}")]
        public virtual async Task<ActionResult> PutAsync(int id, int idPost,[FromBody] PostUpdateInputModel inputModel)
        {
            var blog = await BlogRepository.GetAsync(id);
            
            if(blog == null)
                return NotFound();

            var post = await PostRepository.GetAsync(idPost);
            
            if(post == null)
                return NotFound();          
            
            post.Update(inputModel);

            await PostRepository.PutAsync(post);

            return Ok();
        }

        //DELETE: api/Blogs/id/Posts/idp
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