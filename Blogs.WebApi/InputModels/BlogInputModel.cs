
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Blogs.WebApi.Models;

namespace Blogs.WebApi.InputModels
{
    public class BlogInputModel
    {
        [Required]
        public uint Id { get; set; }

        [Required]
        public string Url { get; set; }

        public ICollection<Post> Posts { get; set; }


        public static implicit operator BlogInputModel(Blog blog)
        {
            if (blog == null)
                return null;

            return new BlogInputModel
            {
                Id = blog.Id,
                Url = blog.Url,
                Posts = new List<Post>()
            };
        } 
    }
}