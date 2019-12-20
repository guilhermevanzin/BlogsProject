using System.ComponentModel.DataAnnotations;

namespace Blogs.WebApi.InputModels
{
    public class BlogUpdateInputModel
    {
        [Required]
        public string Url { get; set; }

        protected BlogUpdateInputModel()
        {
            
        }
        public BlogUpdateInputModel(string url)
        {
            Url = url;
        }

        
    }
}