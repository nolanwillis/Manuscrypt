using Manuscrypt.Shared;
using Microsoft.EntityFrameworkCore;

namespace Manuscrypt.PostService.Data;

public class PostContext : DbContext
{
    public DbSet<Post> Posts { get; set; }
    public DbSet<Event> Events { get; set; }

    public PostContext(DbContextOptions<PostContext> options) : base(options) { }
}
