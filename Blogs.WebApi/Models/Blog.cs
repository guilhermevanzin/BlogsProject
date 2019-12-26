using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Blogs.WebApi.InputModels;

namespace Blogs.WebApi.Models
{
    public class Blog
    {
        public uint Id { get; protected set; }
        public string Url { get; protected set; }
        public ICollection<Post> Posts { get; protected set; }

        protected Blog()
        {        
        }

        public Blog(uint id, string url)
        {   
            Id  = id;
            
            if(string.IsNullOrWhiteSpace(url))
                throw new ArgumentNullException();
            
            Url = url;
            Posts = new List<Post>();
        }
        
        public void SetUrl(string url)
        {
            if(string.IsNullOrWhiteSpace(url))
                throw new ArgumentNullException();
            Url = url;
        }

        public static implicit operator Blog(BlogInputModel inputModel)
        {
            if (inputModel == null)
                return null;

            return new Blog
            {
                Id = inputModel.Id,
                Url = inputModel.Url,
                Posts = new List<Post>()
            };
        }
    }
} 
