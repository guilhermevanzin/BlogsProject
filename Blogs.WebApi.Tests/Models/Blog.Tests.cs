using System.Linq.Expressions;
using System;
using Blogs.WebApi.Models;
using Xunit;

namespace Blogs.WebApi.Tests.Models
{
    public class BlogTests
    {
        [Theory]
        [InlineData(1,"url")]
        [InlineData(111111,"url       url")]
        [InlineData(1,"URL")]
        public void Ctor_WithValidArguments_ShouldCreateAValidBlogInstance(int data1, string data2)
        {
            // arrange
            var id  = (uint)data1;
            var url = data2;
            // act
            var blog = new Blog(id, url);

            // assert
            Assert.NotNull(blog);
            Assert.Equal(id,blog.Id);
            Assert.Equal(url, blog.Url);
            Assert.InRange(id,uint.MinValue,uint.MaxValue);
        }

        [Theory]
        [InlineData("  ")]
        [InlineData("")]
        [InlineData(null)]
        public void Ctor_WithInvalidUrl_ResultsArgumentNullException(string url)
        {
        //Arrange
           var id = (uint)1;
           // Act && Assert
           Assert.Throws<ArgumentNullException>(() => new Blog(id,url));        
        }
    }
}