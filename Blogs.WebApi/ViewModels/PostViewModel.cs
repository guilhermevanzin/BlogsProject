using Blogs.WebApi.Models;
namespace Blogs.WebApi.ViewModels
{
    public class PostViewModel
    {
        public uint Id {get;set;}
        public string Title {get;set;}
        public string Content {get;set;}

        public static implicit operator PostViewModel(Post post)
        {
            if (post == null)
                return null;

            return new PostViewModel
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content
            };
        }
    }
}