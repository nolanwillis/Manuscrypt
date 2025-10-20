using Manuscrypt.Server.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Manuscrypt.Server.Data;

public class ManuscryptContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }
    
    public ManuscryptContext(DbContextOptions<ManuscryptContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Subscription>()
            .HasOne(s => s.Subscriber)
            .WithMany(u => u.Subscriptions)
            .HasForeignKey(s => s.SubscriberId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Subscription>()
            .HasOne(s => s.SubscribedTo)
            .WithMany(u => u.Subscribers)
            .HasForeignKey(s => s.SubscribedToId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
