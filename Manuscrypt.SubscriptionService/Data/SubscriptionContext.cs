using Manuscrypt.Shared;
using Microsoft.EntityFrameworkCore;

namespace Manuscrypt.SubscriptionService.Data;

public class SubscriptionContext : DbContext
{
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<Event> Events { get; set; }

    public SubscriptionContext(DbContextOptions<SubscriptionContext> options) : base(options) { } 
}
