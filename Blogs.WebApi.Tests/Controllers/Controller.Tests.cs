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

namespace Blogs.WebApi.Tests.Controllers
{
    public class ControllerTests
    {
                
        [Fact]
        public async Task GetAllAsync_WithValidBlogInstance_ReturnsOk()
        {
            //Arrange
        
            var blogs = CreateBlogList();
            var mockRepository = new Mock<BlogRepository>();
            mockRepository.Setup(m => m.GetAsync())
                .Returns(Task.FromResult(blogs));
            var controller = GetController(mockRepository.Object);

            //Act
            
            var res = await controller.GetAsync();
            
            //Assert
            
            var objResult  = res.Result as ObjectResult;
            var blogsResult = objResult.Value as IEnumerable<BlogViewModel>; 
            Assert.Equal(200,objResult.StatusCode);
            Assert.NotNull(objResult);
            Assert.Equal(blogs.Count(),blogsResult.Count());
        }

        [Fact]
        public async Task GetAllAsync_WithoutBlogReturned_ReturnsNoContent()
        {
            //Arrange
            
            IEnumerable<Blog> blogs = CreateBlogList(true);
            var mockRepository = new Mock<BlogRepository>();
            mockRepository.Setup(m => m.GetAsync())
                .Returns(Task.FromResult(blogs));

            var controller = GetController(mockRepository.Object);
            
            //Act
            var res = await controller.GetAsync();
            
            //Assert
            Assert.IsType<NoContentResult>(res.Result);
            Assert.Null(res.Value);
            
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GetByIdAsync_WithValidBlogInstance_ReturnsOk(int id)
        {
            //Arrange
            
            IEnumerable<Blog> blogs = CreateBlogList();
            var mockRepository = new Mock<BlogRepository>();
            mockRepository.Setup(m => m.GetAsync(id))
                .Returns(Task.FromResult(blogs.SingleOrDefault(b => b.Id == id)));

            var controller = GetController(mockRepository.Object);
            
            //Act
            var res = await controller.GetAsync(id);
            
            //Assert
            var objResult  = res.Result as ObjectResult;
            var blogResult = objResult.Value as BlogViewModel;
            
            Assert.IsType<ActionResult<BlogViewModel>>(res);
            Assert.Equal(200,objResult.StatusCode);
            Assert.Equal(blogs.SingleOrDefault(b => b.Id == id).Id, blogResult.Id); 
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ReturnsNotFound()
        {
            //Arrange

            int id = 4;
            Blog blog = null;  
            
            var mockRepository = new Mock<BlogRepository>();
            mockRepository.Setup(m => m.GetAsync(id))
                .Returns(Task.FromResult(blog));

            var controller = GetController(mockRepository.Object);    
            
            //Act
            var res = await controller.GetAsync(id);
            
            //Assert
            var objResult  = res.Result as ActionResult; 
            Assert.IsType<ActionResult<BlogViewModel>>(res);
            Assert.NotNull(objResult);
            Assert.Equal(404,(objResult as StatusCodeResult).StatusCode);
        }

        [Theory]
        [InlineData((uint)1)]
        [InlineData((uint)2)]
        [InlineData((uint)3)]
        public async Task PostAsync_WithValidBlogInstance_ReturnsCreated(uint id)
        {
            //Arrange
            BlogInputModel blog = new Blog(id,"www");
            var mockRepository = new Mock<BlogRepository>();
            mockRepository.Setup(m => m.PostAsync(It.IsAny<Blog>()));
            var controller = GetController(mockRepository.Object);
            //Act
            var res = await controller.PostAsync((blog));
            //Assert
            var objResult  = res.Result as CreatedResult;
            Assert.IsType<CreatedResult>(res.Result);
            Assert.Equal($"api/blogs/{id}",objResult.Location);
        }


        [Fact]
        public async Task PutAsync_WithValidBlogInstance_ReturnsOk()
        {
            //Arrange
            var id = 1;
            IEnumerable<Blog> blogs = CreateBlogList();
            BlogUpdateInputModel updateBlog = new BlogUpdateInputModel("www");
            
            var mockRepository = new Mock<BlogRepository>();
            
            mockRepository.Setup(m => m.GetAsync(id))
                .Returns(Task.FromResult(blogs.SingleOrDefault(b => b.Id == id)));
            
            mockRepository.Setup(m => m.PutAsync(It.IsAny<Blog>()));
            
            var controller = GetController(mockRepository.Object);
            
            //Act
            var res = await controller.PutAsync(id,updateBlog);
            
            //Assert
            Assert.IsType<OkResult>(res);
        }

        [Fact]
        public async Task PutAsync_WithInvalidUrl_ReturnsBadRequest()
        {
            //Arrange
            var id = 1;
            BlogUpdateInputModel updateBlog = new BlogUpdateInputModel("");
            
            var mockRepository = new Mock<BlogRepository>();
            
            var controller = GetController(mockRepository.Object);
            
            //Act
            var res = await controller.PutAsync(id,updateBlog);
            
            //Assert
            Assert.IsType<BadRequestResult>(res);
        }

        [Fact]
        public async Task PutAsync_WithoutBlogInstanceReturned_ReturnsNotFound()
        {
            //Arrange

            BlogUpdateInputModel updateBlog = new BlogUpdateInputModel("www");
            
            var mockRepository = new Mock<BlogRepository>();

            var controller = GetController(mockRepository.Object);
            
            //Act
            var res = await controller.PutAsync(It.IsAny<int>(),updateBlog);
            
            //Assert
            Assert.IsType<NotFoundResult>(res);
        }

        [Fact]
        public async Task DeleteAsync_WithValidBlogInstance_ReturnsOK()
        {
            //Arrange
            var id = 1;
            Blog blog = new Blog(1,"www");
            var mockRepository = new Mock<BlogRepository>();
            mockRepository.Setup(m => m.GetAsync(id))
                .Returns(Task.FromResult(blog));
            mockRepository.Setup(m => m.DeleteAsync(It.IsAny<Blog>()));    
            
            var controller = GetController(mockRepository.Object);

            //Act
            var res = await controller.DeleteAsync(1);

            //Assert
            Assert.IsType<OkResult>(res);
            
        }

        [Fact]
        public async Task DeleteAsync_WithoutBlogInstanceReturned_ReturnsBadRequest()
        {
            //Arrange
            Blog blog = new Blog(1,"www");
            
            var mockRepository = new Mock<BlogRepository>();  

            var controller = GetController(mockRepository.Object);

            //Act
            var res = await controller.DeleteAsync(1);

            //Assert
            Assert.IsType<NotFoundResult>(res);
            
        }

        [Fact]
        public async Task GetAsync_WithValidPostInstance_ReturnsOk()
        {
            //Arrange
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

            // Act
            var res =  await controller.GetPostAsync(id);

            //Assert
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
            //Arrange
            var id = 1;
            var mockBlogRepo = new Mock<BlogRepository>();
            var mockPostRepo = new Mock<PostRepository>();
            
            var controller = GetController(mockBlogRepo.Object,mockPostRepo.Object);

            // Act
            var res =  await controller.GetPostAsync(id);

            //Assert
            Assert.IsType<NotFoundResult>(res.Result);
        }

        [Fact]
        public async Task GetAsync_WithoutPostInstanceReturned_ReturnsNoContent()
        {
            //Arrange
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

            // Act
            var res =  await controller.GetPostAsync(id);

            //Assert
            Assert.IsType<NoContentResult>(res.Result);
        }

        [Fact]
        public async Task PostAsync_WithValidPostInstance_ReturnsCreated()
        {
            //Arrange
            var blog = new Blog(1,"www");
            var post = new Post((uint)1,"Title1","Content1",blog);
            var id = 1;
            var mockBlogRepo = new Mock<BlogRepository>();
            mockBlogRepo.Setup(m => m.GetAsync(id))
                .Returns(Task.FromResult(blog));
            
            var mockPostRepo = new Mock<PostRepository>();
            mockPostRepo.Setup(m => m.PostAsync(It.IsAny<Post>()));
            
            var controller = GetController(mockBlogRepo.Object,mockPostRepo.Object);

            // Act
            var res =  await controller.PostAsync(id,(PostInputModel) post);

            //Assert
            var objResult  = res.Result as CreatedResult;
            Assert.IsType<CreatedResult>(res.Result);
            Assert.Equal($"api/Blogs/{id}/Posts",objResult.Location);
        }

        [Fact]
        public async Task PostAsync_WithoutBlogInstanceReturned_ReturnsNotFound()
        {
            //Arrange
            var id = 1;
            Blog blog = null;
            var mockBlogRepo = new Mock<BlogRepository>();
            mockBlogRepo.Setup(m => m.GetAsync(id))
                .Returns(Task.FromResult(blog));
            
            var mockPostRepo = new Mock<PostRepository>();
            
            var controller = GetController(mockBlogRepo.Object,mockPostRepo.Object);

            // Act
            var res =  await controller.PostAsync(id,(PostInputModel) It.IsAny<Post>());

            //Assert
            Assert.IsType<NotFoundResult>(res.Result);
        }

        [Fact]
        public async Task PutAsync_WithValidPostInstance_ReturnsOk()
        {
            //Arrange
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
            
            //Act
            var res = await controller.PutAsync(id,idPost,updatePost);
            
            //Assert
            Assert.IsType<OkResult>(res);

        }

        [Fact]
        public async Task PutPostAsync_WithoutBlogInstanceReturned_ReturnsNotFound()
        {
            //Arrange
            var id = 1;
            var idPost = 1;
            var updatePost = new PostUpdateInputModel("Title2","Content2",(uint)1);
            
            var mockBlogRepo = new Mock<BlogRepository>();
            var mockPostRepo = new Mock<PostRepository>();
            
            var controller = GetController(mockBlogRepo.Object,mockPostRepo.Object);
            
            //Act
            var res = await controller.PutAsync(id,idPost,updatePost);
            
            //Assert
            Assert.IsType<NotFoundResult>(res);

        }

         [Fact]
         public async Task PutAsync_WithoutPostInstanceReturned_ReturnsNotFound()
        {
            //Arrange
            var id = 1;
            var idPost = 1;
            var blog = new Blog(1,"www");
            var updatePost = new PostUpdateInputModel("Title2","Content2",(uint)1);
            
            var mockBlogRepo = new Mock<BlogRepository>();
            mockBlogRepo.Setup(m => m.GetAsync(id))
                .Returns(Task.FromResult(blog));
        
            var mockPostRepo = new Mock<PostRepository>();
            
            var controller = GetController(mockBlogRepo.Object,mockPostRepo.Object);
            
            //Act
            var res = await controller.PutAsync(id,idPost,updatePost);
            
            //Assert
            Assert.IsType<NotFoundResult>(res);
        }

        [Fact]
        public async Task DeleteAsync_WithValidPostInstance_ReturnsOk()
        {
            //Arrange
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
            
            //Act
            var res = await controller.DeleteAsync(id,idPost);
            
            //Assert
            Assert.IsType<OkResult>(res);
        }

        [Fact]
        public async Task DeleteAsync_WithoutBlogInstanceReturned_ReturnsNotFound()
        {
            //Arrange
            var id = 1;
            var idPost = 1;
            
            var mockBlogRepo = new Mock<BlogRepository>();
            var mockPostRepo = new Mock<PostRepository>();
            var controller = GetController(mockBlogRepo.Object,mockPostRepo.Object);
            
            //Act
            var res = await controller.DeleteAsync(id,idPost);
            
            //Assert
            Assert.IsType<NotFoundResult>(res);
        }

        [Fact]
        public async Task DeleteAsync_WithoutPostInstanceReturned_ReturnsNotFound()
        {
            //Arrange
            var id = 1;
            var idPost = 1;
            var blog = new Blog(1,"www");
            var mockBlogRepo = new Mock<BlogRepository>();
            mockBlogRepo.Setup(m => m.GetAsync(id))
                .Returns(Task.FromResult(blog));
            var mockPostRepo = new Mock<PostRepository>();
            var controller = GetController(mockBlogRepo.Object,mockPostRepo.Object);
            
            //Act
            var res = await controller.DeleteAsync(id,idPost);
            
            //Assert
            Assert.IsType<NotFoundResult>(res);
        }

#region
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
            var controller = new BlogsController(options);
            controller.SetBlogRepository(blogRepository);
            return controller;
        }
        private BlogsController GetController(BlogRepository blogRepository,PostRepository postRepository) 
        {
            var options =  new DbContextOptionsBuilder<BloggingContext>()
            .UseInMemoryDatabase("postsdb")
            .Options;
            var controller = new BlogsController(options);
            controller.SetBlogRepository(blogRepository);
            controller.SetPostRepository(postRepository);
            return controller;
        }
#endregion        
    }
}