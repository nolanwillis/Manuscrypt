using Manuscrypt.Shared;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Manuscrypt.AuthService.Data;

public class AuthContext : IdentityDbContext<User>
{
    public DbSet<Event> Events { get; set; }

    public AuthContext(DbContextOptions<AuthContext> options) : base(options) { }
}
