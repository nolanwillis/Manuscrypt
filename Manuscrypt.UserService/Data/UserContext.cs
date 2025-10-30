using Manuscrypt.Shared;
using Microsoft.EntityFrameworkCore;

namespace Manuscrypt.UserService.Data;

public class UserContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Event> Events { get; set; }

    public UserContext(DbContextOptions<UserContext> options) : base(options) { }
}
