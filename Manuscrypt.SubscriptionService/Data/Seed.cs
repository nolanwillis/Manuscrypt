using Microsoft.EntityFrameworkCore;

namespace Manuscrypt.SubscriptionService.Data;

public static class Seed
{
    public static async Task SeedSubscriptionsAsync(SubscriptionContext context, int userCount)
    {
        foreach (var subscription in context.Subscriptions)
        {
            context.Subscriptions.Remove(subscription);
        }
        await context.SaveChangesAsync();

        var subscriptions = new List<Subscription>();

        // Have every user subscribe to every other user.
        for (int i = 1; i <= userCount; i++)
        {
            for (int j = 1; j <= userCount; j++)
            {
                if (i == j) continue; // Prevent self-subscription

                subscriptions.Add(new Subscription
                {
                    SubscriberId = i,
                    SubscribedToId = j,
                    CreatedAt = DateTime.UtcNow
                });
            }
        }

        await context.Subscriptions.AddRangeAsync(subscriptions);
        await context.SaveChangesAsync();
    }
}
