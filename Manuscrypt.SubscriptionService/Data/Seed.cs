using Microsoft.EntityFrameworkCore;

namespace Manuscrypt.SubscriptionService.Data;

public static class Seed
{
    public static async Task SeedSubscriptionsAsync(SubscriptionContext context, int subscriptionCount)
    {
        if (await context.Subscriptions.AnyAsync())
        {
            return;
        }

        var rand = new Random();
        var subscriptions = new List<Subscription>();

        for (int i = 1; i <= subscriptionCount; i++)
        {
            int subscriberId = rand.Next(1, 1000);
            int subscribedToId;
            do
            {
                subscribedToId = rand.Next(1, 1000);
            } 
            while (subscribedToId == subscriberId); // Prevent self-subscription

            subscriptions.Add(new Subscription
            {
                SubscriberId = subscriberId,
                SubscribedToId = subscribedToId,
                CreatedAt = DateTime.UtcNow.AddDays(-rand.Next(0, 365))
            });
        }

        await context.Subscriptions.AddRangeAsync(subscriptions);
        await context.SaveChangesAsync();
    }
}
