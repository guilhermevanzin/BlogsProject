
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

        protected BlogInputModel()
        {
        }

        public BlogInputModel(uint id, string url)
        {
            Id = id;
            Url = url;
        }
    }
}