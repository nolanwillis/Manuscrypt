using Manuscrypt.Shared;
using Microsoft.EntityFrameworkCore;

namespace Manuscrypt.CommentService.Data;

public class CommentContext : DbContext
{
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Event> Events { get; set; }

    public CommentContext(DbContextOptions<CommentContext> options) : base(options) {}
}
