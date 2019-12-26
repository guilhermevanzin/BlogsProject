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
        public void Ctor_WithValidArguments_ShouldCreateAValidBlogInstance(int id, string url)
        {
            var blog = new Blog((uint)id, url);

            Assert.NotNull(blog);
            Assert.Equal((uint)id,blog.Id);
            Assert.Equal(url, blog.Url);
            Assert.InRange((uint)id,uint.MinValue,uint.MaxValue);
        }

        [Theory]
        [InlineData("  ")]
        [InlineData("")]
        [InlineData(null)]
        public void Ctor_WithInvalidUrl_ResultsArgumentNullException(string url)
        {
           var id = (uint)1;
           
           Assert.Throws<ArgumentNullException>(() => new Blog(id,url));        
        }

        [Theory]
        [InlineData("http")]
        [InlineData("www")]
        [InlineData("https")]
        public void SetUrl_WithValidUrl_ShouldSetAValidBlogUrl(string url)
        {
            var blog = new Blog(1,"w");
            
            blog.SetUrl(url);
            
            Assert.Equal(url,blog.Url);
        }

        [Theory]
        [InlineData("  ")]
        [InlineData("")]
        [InlineData(null)]
        public void SetUrl_WithInvalidUrl_ReturnsArgumentNullException(string url)
        {
            var blog = new Blog(1,"w");
            
            Assert.Throws<ArgumentNullException>(() => blog.SetUrl(url));
        }
    }
}