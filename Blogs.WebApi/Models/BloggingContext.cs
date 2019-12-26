using Microsoft.EntityFrameworkCore;

namespace Blogs.WebApi.Models
{
    public class BloggingContext:DbContext
    {
        public BloggingContext(DbContextOptions<BloggingContext> options)
        : base(options)
        {
        }

        public virtual DbSet<Blog> Blogs {get;set;}
        public virtual DbSet<Post> Posts {get;set;}
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Blog>()
            .HasIndex(b => b.Url)
            .IsUnique();
        }
    }
}