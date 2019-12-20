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

        public static implicit operator PostInputModel (Post inputModel)
        {
            if (inputModel == null)
                return null;

            return new PostInputModel
            {
                Id = inputModel.Id,
                Title = inputModel.Title,
                Content = inputModel.Content
            };
        }
    }
}