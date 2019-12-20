using System.ComponentModel.DataAnnotations;

namespace Blogs.WebApi.InputModels
{
    public class PostUpdateInputModel
    {
        [Required]
        public string Title {get;set;}
        [Required]
        public string Content {get;set;}
        [Required]
        public uint BlogId {get;set;}
    
    protected PostUpdateInputModel()
    {
    }

    public PostUpdateInputModel(string title, string content, uint blogId)
    {
        Title = title;
        Content = content;
        BlogId = blogId;
    }

    }
}