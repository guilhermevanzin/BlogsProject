using System.ComponentModel.DataAnnotations;

namespace Blogs.WebApi.InputModels
{
    public class PostUpdateInputModel
    {
        public string Title {get;set;}
        public string Content {get;set;}
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