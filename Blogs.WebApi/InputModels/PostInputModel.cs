using System.ComponentModel.DataAnnotations;
using Blogs.WebApi.Models;

namespace Blogs.WebApi.InputModels
{
    public class PostInputModel
    {
        [Required]
        public uint Id {get; set;}
        [Required]
        public string Title {get;set;}
        [Required]
        public string Content {get;set;}

        protected PostInputModel()
        {
        }

        public PostInputModel(uint id, string title, string content)
        {
            Id = id;
            Title = title;
            Content = content;
        }
    }
}