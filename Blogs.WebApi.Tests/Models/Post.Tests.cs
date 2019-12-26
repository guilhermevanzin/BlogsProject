using System;
using Blogs.WebApi.Models;
using Xunit;
namespace Blogs.WebApi.Tests.Models
{
    public class PostTests
    {
        [Theory]
        [InlineData(1,"title","this is a content")]
        [InlineData(1,"title    ","1232132132131232131231311312312")]
        [InlineData(1,"qweqeqweqweqweqw                wqeqweqeqweqweqw                 qweqwe ","this is a content")]
        public void Ctor_WithValidArguments_ShouldCreateAValidPostInstance(int id, string title,string content)
        {            
            var blog = CreateNewBlog();
            var uId = (uint)id;
            var post =  new Post(uId,title,content,blog);

            Assert.NotNull(post);
            Assert.Equal(uId,post.Id);
            Assert.Equal(title,post.Title);
            Assert.Equal(content,post.Content);
            Assert.Same(blog,post.Blog);    
        }

        [Theory]
        [InlineData("               ")]
        [InlineData("")]
        [InlineData(null)]
        public void Ctor_WithInvalidTitle_ReturnsArgumentNullException(string title)
        {   
            var blog = CreateNewBlog();
            var id = (uint)1;
            var content = "test";
               
            Assert.Throws<ArgumentNullException>(() => new Post(id,title,content,blog));
        }
        
        [Theory]
        [InlineData("               ")]
        [InlineData(" ")]
        [InlineData("")]
        [InlineData(null)]
        public void Ctor_WithInvalidContent_ReturnsArgumentNullException(string content)
        {
            var blog = CreateNewBlog();
            var id = (uint)1;
            var title = "test";
              
            Assert.Throws<ArgumentNullException>(() => new Post(id,title,content,blog));
        }

        [Fact]
        public void Ctor_WithNullBlogInstance_ReturnsArgumentNullException()
        {
            Blog blog = null;
            var id = (uint)1;
            var title = "test";
            var content = "content test";   
            
            Assert.Throws<ArgumentNullException>(() => new Post(id,title,content,blog));
        }

        [Fact]
        public void SetBlog_WhitValidBlogInstance_ShouldSetAValidBlog()
        {
            var blog = CreateNewBlog();
            var newBlog = CreateNewBlog((uint)2,"http");
            var post = new Post((uint)1,"Title","Content",blog);

            post.SetBlog(newBlog);

            Assert.Equal(newBlog.Id,post.BlogId);
            Assert.Equal(newBlog.Url,post.Blog.Url);
            Assert.Equal(newBlog,post.Blog);
        }

        [Fact]
        public void SetBlog_WhitInvalidBlogInstance_ReturnsArgumentNullException()
        {
            Blog blog = null;
            var post = new Post((uint)1,"Title","Content",CreateNewBlog());

            Assert.Throws<ArgumentNullException>(()=>post.SetBlog(blog));
        }

        [Theory]
        [InlineData("cont1")]
        [InlineData("cont2")]
        [InlineData("cont3")]
        public void SetContent_WithValidContent_SholdSetAValidContent(string content)
        {
            var post = new Post((uint)1,"Title","Content",CreateNewBlog());

            post.SetContent(content);

            Assert.Equal(content,post.Content);
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        public void SetContent_WithInvalidContent_ReturnsArgumentNullException(string content)
        {
            var post = new Post((uint)1,"Title","Content",CreateNewBlog());

            Assert.Throws<ArgumentNullException>(()=>post.SetContent(content));
        }

        [Theory]
        [InlineData("Tit1")]
        [InlineData("Tit2")]
        [InlineData("Tit3")]
        public void SetTitle_WithValidTitle_SholdSetAValidTitle(string title)
        {
            var post = new Post((uint)1,"Title","Content",CreateNewBlog());

            post.SetTitle(title);

            Assert.Equal(title,post.Title);
        }

        [Fact]
        public void Update_WithValidTitle_ShouldUpdateAPostInstance()
        {
            var blog = CreateNewBlog();
            var post = new Post((uint)1,"Title","Content",blog);
            var title = "New Title";
            
            post.Update(blog.Id,post.Content,title);

            Assert.Equal(title,post.Title);
        }

        [Fact]
        public void Update_WithValidContent_ShouldUpdateAPostInstance()
        {
            var blog = CreateNewBlog();
            var post = new Post((uint)1,"Title","Content",blog);
            var content = "New Content";
            
            post.Update(blog.Id,content,post.Title);

            Assert.Equal(content,post.Content);
        }

        [Theory]
        [InlineData(uint.MinValue)]
        [InlineData(uint.MaxValue)]
        [InlineData(2)]
        public void Update_WithValidBlogId_ShouldUpdateAPostInstance(uint blogId)
        {
            var blog = CreateNewBlog();
            var post = new Post((uint)1,"Title","Content",blog);
            var newBlogId = blogId;
            
            post.Update(blogId,post.Content,post.Title);

            Assert.Equal(newBlogId,post.BlogId);
        }

        private Blog CreateNewBlog(uint id = 1, string url = "www")
            => new Blog(id, url);
    }
}