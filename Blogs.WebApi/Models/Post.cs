using System;
using Blogs.WebApi.InputModels;

namespace Blogs.WebApi.Models
{
    public class Post
    {
        public uint Id { get; protected set; }
        public string Title { get; protected set; }
        public string Content { get; protected set; }
        public uint BlogId { get; protected set; }
        public Blog Blog { get; protected set; }

        protected Post()
        {
        }

        public Post(uint id, string title, string content, Blog blog)
        {
            Id = id;
            SetTitle(title);
            SetContent(content);
            SetBlog(blog);
        }

        public void SetBlog(Blog blog)
        {
            if (blog == null)
                throw new ArgumentNullException(nameof(blog));

            BlogId = blog.Id;
            Blog = blog;
        }

        public void SetContent(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentNullException(nameof(content));

            Content = content;
        }

        public void SetTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentNullException(nameof(title));

            Title = title;
        }

        public void Update(PostUpdateInputModel inputModel)
        {
                if(inputModel.BlogId != 0)
                    BlogId = inputModel.BlogId; 
                Content = inputModel.Content;
                Title = inputModel.Title;
        }

        public static implicit operator Post(PostInputModel inputModel)
        {
            if (inputModel == null)
                return null;

            return new Post
            {
                Id = inputModel.Id,
                Title = inputModel.Title,
                Content = inputModel.Content
            };
        }
    }
}