using Manuscrypt.Server.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Manuscrypt.Server.Data;

public class ManuscryptContext : DbContext
{
    public DbSet<Channel> Channels => Set<Channel>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Subscription> Subscriptions => Set<Subscription>();
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<Edit> Edits => Set<Edit>();
    public DbSet<PostTag> PostTags => Set<PostTag>();

    public ManuscryptContext(DbContextOptions<ManuscryptContext> options) : base(options) {}
}
