using Manuscrypt.Server.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Manuscrypt.Server.Data;

public class ManuscryptContext : DbContext
{
    public ManuscryptContext(DbContextOptions<ManuscryptContext> options) : base(options) {}
    
    public DbSet<Channel> Channels { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Edit> Edits { get; set; }
    public DbSet<PostTag> PostTags { get; set; }
    
}
