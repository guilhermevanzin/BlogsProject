using System.Linq;
using System.Collections.Generic;
using Xunit;
using Moq;
using Blogs.WebApi.Controllers;
using Blogs.WebApi.Models;
using System.Threading.Tasks;
using Blogs.WebApi.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Blogs.WebApi.ViewModels;
using Blogs.WebApi.InputModels;
using Blogs.WebApi.Interfaces;

namespace Blogs.WebApi.Tests.Controllers
{
    public class ControllerTests
    {          
        [Fact]
        public async Task GetAllAsync_WithValidBlogInstance_ReturnsOk()
        {   
            var blogs = CreateBlogList();
            var mockRepository = new Mock<BlogRepository>();
            
            mockRepository.Setup(m => m.GetAsync())
                .Returns(Task.FromResult(blogs));
            
            var controller = GetController(mockRepository.Object);
            
            var res = await controller.GetAsync();
            
            var objResult  = res.Result as ObjectResult;
            var blogsResult = objResult.Value as IEnumerable<Blog>; 
            Assert.Equal(200,objResult.StatusCode);
            Assert.NotNull(objResult);
            Assert.Equal(blogs.Count(),blogsResult.Count());
        }

        [Fact]
        public async Task GetAllAsync_WithoutBlogReturned_ReturnsNoContent()
        {   
            IEnumerable<Blog> blogs = CreateBlogList(true);
            var mockRepository = new Mock<BlogRepository>();
            
            mockRepository.Setup(m => m.GetAsync())
                .Returns(Task.FromResult(blogs));

            var controller = GetController(mockRepository.Object);
            
            var res = await controller.GetAsync();
            
            Assert.IsType<NoContentResult>(res.Result);
            Assert.Null(res.Value);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GetByIdAsync_WithValidBlogInstance_ReturnsOk(int id)
        { 
            IEnumerable<Blog> blogs = CreateBlogList();
            var mockRepository = new Mock<BlogRepository>();
            
            mockRepository.Setup(m => m.GetAsync(id))
                .Returns(Task.FromResult(blogs.SingleOrDefault(b => b.Id == id)));

            var controller = GetController(mockRepository.Object);
            
            var res = await controller.GetAsync(id);
            
            var objResult  = res.Result as ObjectResult;
            var blogResult = objResult.Value as Blog;
            Assert.IsType<ActionResult<Blog>>(res);
            Assert.Equal(200,objResult.StatusCode);
            Assert.Equal(blogs.SingleOrDefault(b => b.Id == id).Id, blogResult.Id); 
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ReturnsNotFound()
        {
            int id = 4;
            Blog blog = null;     
            var mockRepository = new Mock<BlogRepository>();
            
            mockRepository.Setup(m => m.GetAsync(id))
                .Returns(Task.FromResult(blog));

            var controller = GetController(mockRepository.Object);    
            
            var res = await controller.GetAsync(id);
            
            var objResult  = res.Result as ActionResult; 
            Assert.IsType<ActionResult<Blog>>(res);
            Assert.NotNull(objResult);
            Assert.Equal(404,(objResult as StatusCodeResult).StatusCode);
        }

        [Theory]
        [InlineData((uint)1)]
        [InlineData((uint)2)]
        [InlineData((uint)3)]
        public async Task PostAsync_WithValidBlogInstance_ReturnsCreated(uint id)
        {
            BlogInputModel blog = new BlogInputModel(id, "www");
            var mockRepository = new Mock<BlogRepository>();
            
            mockRepository.Setup(m => m.PostAsync(It.IsAny<Blog>()));
            
            var controller = GetController(mockRepository.Object);
            
            var res = await controller.PostAsync((blog));
            
            var objResult  = res.Result as CreatedResult;
            Assert.IsType<CreatedResult>(res.Result);
            Assert.Equal($"api/blogs/{id}",objResult.Location);
        }

        [Fact]
        public async Task PutAsync_WithValidBlogInstance_ReturnsOk()
        {
            var id = 1;
            IEnumerable<Blog> blogs = CreateBlogList();
            BlogUpdateInputModel updateBlog = new BlogUpdateInputModel("www");
            var mockRepository = new Mock<BlogRepository>();
            
            mockRepository.Setup(m => m.GetAsync(id))
                .Returns(Task.FromResult(blogs.SingleOrDefault(b => b.Id == id)));
            
            mockRepository.Setup(m => m.PutAsync(It.IsAny<Blog>()));
            
            var controller = GetController(mockRepository.Object);            
           
            var res = await controller.PutAsync(id,updateBlog);
               
            Assert.IsType<OkResult>(res);
        }

        [Fact]
        public async Task PutAsync_WithInvalidUrl_ReturnsBadRequest()
        {
            var id = 1;
            BlogUpdateInputModel updateBlog = new BlogUpdateInputModel("");   
            var mockRepository = new Mock<BlogRepository>();
            
            var controller = GetController(mockRepository.Object);
           
            var res = await controller.PutAsync(id,updateBlog);
    
            Assert.IsType<BadRequestResult>(res);
        }

        [Fact]
        public async Task PutAsync_WithoutBlogInstanceReturned_ReturnsNotFound()
        {        
            BlogUpdateInputModel updateBlog = new BlogUpdateInputModel("www");   
            var mockRepository = new Mock<BlogRepository>();
            var controller = GetController(mockRepository.Object);
            
            var res = await controller.PutAsync(It.IsAny<int>(),updateBlog);
            
            Assert.IsType<NotFoundResult>(res);
        }

        [Fact]
        public async Task DeleteAsync_WithValidBlogInstance_ReturnsOK()
        {
            var id = 1;
            Blog blog = new Blog(1,"www");
            var mockRepository = new Mock<BlogRepository>();
            
            mockRepository.Setup(m => m.GetAsync(id))
                .Returns(Task.FromResult(blog));
            
            mockRepository.Setup(m => m.DeleteAsync(It.IsAny<Blog>()));    
            
            var controller = GetController(mockRepository.Object);

            var res = await controller.DeleteAsync(1);
            
            Assert.IsType<OkResult>(res);            
        }

        [Fact]
        public async Task DeleteAsync_WithoutBlogInstanceReturned_ReturnsBadRequest()
        {
            Blog blog = new Blog(1,"www");   
            var mockRepository = new Mock<BlogRepository>();  

            var controller = GetController(mockRepository.Object);

            var res = await controller.DeleteAsync(1);

            Assert.IsType<NotFoundResult>(res);
        }

        [Fact]
        public async Task GetAsync_WithValidPostInstance_ReturnsOk()
        {
            var posts = CreatePostList();
            var blog = new Blog(1,"www");
            var id = 1;
            var mockBlogRepo = new Mock<BlogRepository>();
            
            mockBlogRepo.Setup(m => m.GetAsync(id))
                .Returns(Task.FromResult(blog));
            
            var mockPostRepo = new Mock<PostRepository>();
            
            mockPostRepo.Setup(m => m.GetAsync(It.IsAny<Blog>()))
                .Returns(Task.FromResult(posts));
            
            var controller = GetController(mockBlogRepo.Object,mockPostRepo.Object);

            var res =  await controller.GetPostAsync(id);
            
            var objResult = res.Result as ObjectResult;
            var postsResult = objResult.Value as IEnumerable<PostViewModel>;
            Assert.IsType<OkObjectResult>(res.Result);
            Assert.NotNull(objResult.Value);
            Assert.Equal(posts.Count(),postsResult.Count());
            Assert.Equal(posts.SingleOrDefault(p => p.Id == 1).BlogId, blog.Id);
            Assert.Equal(posts.SingleOrDefault(p => p.Id == 1).Title, postsResult.SingleOrDefault(p => p.Id == 1).Title);
        }

        [Fact]
        public async Task GetAsync_WithoutBlogInstanceReturned_ReturnsNotFound()
        { 
            var id = 1;
            var mockBlogRepo = new Mock<BlogRepository>();
            var mockPostRepo = new Mock<PostRepository>();
            
            var controller = GetController(mockBlogRepo.Object,mockPostRepo.Object);
            
            var res =  await controller.GetPostAsync(id);
            
            Assert.IsType<NotFoundResult>(res.Result);
        }

        [Fact]
        public async Task GetAsync_WithoutPostInstanceReturned_ReturnsNoContent()
        {
            var id = 1;
            var blog = new Blog(1,"www");
            var posts = CreatePostList(true);
            var mockBlogRepo = new Mock<BlogRepository>();
            
            mockBlogRepo.Setup(m => m.GetAsync(id))
                .Returns(Task.FromResult(blog));

            var mockPostRepo = new Mock<PostRepository>();
            
            mockPostRepo.Setup(m => m.GetAsync(It.IsAny<Blog>()))
                .Returns(Task.FromResult(posts));
            
            var controller = GetController(mockBlogRepo.Object,mockPostRepo.Object);

            var res =  await controller.GetPostAsync(id);
           
            Assert.IsType<NoContentResult>(res.Result);
        }

        [Fact]
        public async Task PostAsync_WithValidPostInstance_ReturnsCreated()
        {
            var blog = new Blog(1,"www");
            var inputModel = new PostInputModel((uint)1,"Title1","Content1");
            var id = 1;
            var mockBlogRepo = new Mock<BlogRepository>();
            
            mockBlogRepo.Setup(m => m.GetAsync(id))
                .Returns(Task.FromResult(blog));
            
            var mockPostRepo = new Mock<PostRepository>();
            
            mockPostRepo.Setup(m => m.PostAsync(It.IsAny<Post>()));
            
            var controller = GetController(mockBlogRepo.Object,mockPostRepo.Object);
 
            var res =  await controller.PostAsync(id,inputModel);

            var objResult  = res.Result as CreatedResult;
            Assert.IsType<CreatedResult>(res.Result);
            Assert.Equal($"api/Blogs/{id}/Posts",objResult.Location);
        }

        [Fact]
        public async Task PostAsync_WithoutBlogInstanceReturned_ReturnsNotFound()
        {
            var id = 1;
            Blog blog = null;
            var mockBlogRepo = new Mock<BlogRepository>();
            
            mockBlogRepo.Setup(m => m.GetAsync(id))
                .Returns(Task.FromResult(blog));
            
            var mockPostRepo = new Mock<PostRepository>();
            
            var controller = GetController(mockBlogRepo.Object,mockPostRepo.Object);

            var res =  await controller.PostAsync(id, It.IsAny<PostInputModel>());
            
            Assert.IsType<NotFoundResult>(res.Result);
        }

        [Fact]
        public async Task PutAsync_WithValidPostInstance_ReturnsOk()
        {
            var id = 1;
            var idPost = 1;
            var blog = new Blog(1,"www");
            var post = new Post(1,"Title1","Content1",blog);
            var updatePost = new PostUpdateInputModel("Title2","Content2",(uint)1);
            var mockBlogRepo = new Mock<BlogRepository>();
            
            mockBlogRepo.Setup(m => m.GetAsync(id))
                .Returns(Task.FromResult(blog));
        
            var mockPostRepo = new Mock<PostRepository>();
            
            mockPostRepo.Setup(m => m.GetAsync(idPost))
                .Returns(Task.FromResult(post));
            
            mockPostRepo.Setup(m => m.PutAsync(It.IsAny<Post>()));
            
            var controller = GetController(mockBlogRepo.Object,mockPostRepo.Object);
            
            var res = await controller.PutAsync(id,idPost,updatePost);
            
            Assert.IsType<OkResult>(res);
        }

        [Fact]
        public async Task PutPostAsync_WithoutBlogInstanceReturned_ReturnsNotFound()
        {
            var id = 1;
            var idPost = 1;
            var updatePost = new PostUpdateInputModel("Title2","Content2",(uint)1);   
            var mockBlogRepo = new Mock<BlogRepository>();
            var mockPostRepo = new Mock<PostRepository>();
            
            var controller = GetController(mockBlogRepo.Object,mockPostRepo.Object);
            
            var res = await controller.PutAsync(id,idPost,updatePost);
            
            Assert.IsType<NotFoundResult>(res);
        }

         [Fact]
         public async Task PutAsync_WithoutPostInstanceReturned_ReturnsNotFound()
        {
            var id = 1;
            var idPost = 1;
            var blog = new Blog(1,"www");
            var updatePost = new PostUpdateInputModel("Title2","Content2",(uint)1);   
            var mockBlogRepo = new Mock<BlogRepository>();
           
            mockBlogRepo.Setup(m => m.GetAsync(id))
                .Returns(Task.FromResult(blog));
        
            var mockPostRepo = new Mock<PostRepository>();
            
            var controller = GetController(mockBlogRepo.Object,mockPostRepo.Object);

            var res = await controller.PutAsync(id,idPost,updatePost);
                        
            Assert.IsType<NotFoundResult>(res);
        }

        [Fact]
        public async Task DeleteAsync_WithValidPostInstance_ReturnsOk()
        {
            var id = 1;
            var idPost = 1;
            var blog = new Blog(1,"www");
            var post = new Post(1,"Title1","Content1",blog);   
            var mockBlogRepo = new Mock<BlogRepository>();
            
            mockBlogRepo.Setup(m => m.GetAsync(id))
                .Returns(Task.FromResult(blog));
        
            var mockPostRepo = new Mock<PostRepository>();
            
            mockPostRepo.Setup(m => m.GetAsync(idPost))
                .Returns(Task.FromResult(post));
            
            mockPostRepo.Setup(m => m.DeleteAsync(It.IsAny<Post>()));
            
            var controller = GetController(mockBlogRepo.Object,mockPostRepo.Object);
            
            var res = await controller.DeleteAsync(id,idPost);
            
            Assert.IsType<OkResult>(res);
        }

        [Fact]
        public async Task DeleteAsync_WithoutBlogInstanceReturned_ReturnsNotFound()
        {
            var id = 1;
            var idPost = 1;   
            var mockBlogRepo = new Mock<BlogRepository>();
            var mockPostRepo = new Mock<PostRepository>();
            
            var controller = GetController(mockBlogRepo.Object,mockPostRepo.Object);
            
            var res = await controller.DeleteAsync(id,idPost);
            
            Assert.IsType<NotFoundResult>(res);
        }

        [Fact]
        public async Task DeleteAsync_WithoutPostInstanceReturned_ReturnsNotFound()
        {
            var id = 1;
            var idPost = 1;
            var blog = new Blog(1,"www");
            var mockBlogRepo = new Mock<BlogRepository>();
            
            mockBlogRepo.Setup(m => m.GetAsync(id))
                .Returns(Task.FromResult(blog));
            
            var mockPostRepo = new Mock<PostRepository>();
            
            var controller = GetController(mockBlogRepo.Object,mockPostRepo.Object);
            
            var res = await controller.DeleteAsync(id,idPost);
            
            Assert.IsType<NotFoundResult>(res);
        }

        private IEnumerable<Blog> CreateBlogList(bool emptyList = false)
        {
           if(emptyList)
            return new List<Blog>();  
           
           IEnumerable<Blog> blogs = new List<Blog>()
            {
                new Blog((uint)3,"https"),
                new Blog((uint)1,"www"),
                new Blog((uint)2,"http")
            }; 
            
            return blogs;
        }

        private IEnumerable<Post> CreatePostList(bool emptyList = false)
        {
           if(emptyList)
            return new List<Post>();  
           
           IEnumerable<Post> posts = new List<Post>()
            {
                new Post((uint)3,"Title3","Content3",new Blog(1,"www")),
                new Post((uint)1,"Title1","Content1",new Blog(1,"www")),
                new Post((uint)2,"Title2","Content2",new Blog(1,"www"))
            }; 
            
            return posts;
        }

        private BlogsController GetController(BlogRepository blogRepository) 
        {
            var options =  new DbContextOptionsBuilder<BloggingContext>()
            .UseInMemoryDatabase("postsdb")
            .Options;
            var controller = new BlogsController(options,blogRepository);
            
            return controller;
        }

        private BlogsController GetController(IBlogRepository blogRepository,IPostRepository postRepository) 
        {
            var options =  new DbContextOptionsBuilder<BloggingContext>()
            .UseInMemoryDatabase("postsdb")
            .Options;
            
            var controller = new BlogsController(options,blogRepository,postRepository);
            
            return controller;
        }       
    }
}