using System.Collections.Generic;
using Blogs.WebApi.InputModels;
using Blogs.WebApi.Models;

namespace Blogs.WebApi.ViewModels
{
    public class BlogViewModel
    {
        public uint Id {get;set;}
        public string Url {get;set;}

        public static implicit operator BlogViewModel(BlogInputModel blog)
        {
            if (blog == null)
                return null;

            return new BlogViewModel
            {
                Id = blog.Id,
                Url = blog.Url
            };
        }
    }
}
