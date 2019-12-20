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
            //Arrange
            var blog = CreateNewBlog();
            var uId = (uint)id;
            
            //act
            var post =  new Post(uId,title,content,blog);

            //Assert'
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
            //Arrange
            var blog = CreateNewBlog();
            var id = (uint)1;
            var content = "test";
            
            //Act && Assert
            Assert.Throws<ArgumentNullException>(() => new Post(id,title,content,blog));
        }
        
        [Theory]
        [InlineData("               ")]
        [InlineData(" ")]
        [InlineData("")]
        [InlineData(null)]
        public void Ctor_WithInvalidContent_ReturnsArgumentNullException(string content)
        {
            //Arrange
            var blog = CreateNewBlog();
            var id = (uint)1;
            var title = "test";
            
            //Act && Assert
            Assert.Throws<ArgumentNullException>(() => new Post(id,title,content,blog));
        }

        [Fact]
        public void Ctor_WithNullBlogInstance_ReturnsArgumentNullException()
        {
            //Arrange
            Blog blog = null;
            var id = (uint)1;
            var title = "test";
            var content = "content test";
            
            //Act && Assert
            Assert.Throws<ArgumentNullException>(() => new Post(id,title,content,blog));
        }

        private Blog CreateNewBlog(uint id = 1, string url = "www")
            => new Blog(id, url);
    }
}