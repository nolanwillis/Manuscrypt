using Microsoft.AspNetCore.Identity;

namespace Manuscrypt.AuthService.Data;

public class Seed
{
    public static async Task SeedUsersAsync(UserManager<User> userManager, int userCount)
    {
        foreach (var user in userManager.Users)
        {
            await userManager.DeleteAsync(user);
        }

        for (int i = 1; i <= userCount; i++)
        {
            var user = new User
            {
                SeedId = i,
                UserName = $"user{i}",
                Email = $"user{i}@somewhere.com",
                DisplayName = $"User{i}",
                PhotoUrl = $"PhotoUrl{i}.com",
                Description = $"User {i}'s description.",
                CreatedAt = DateTime.UtcNow.AddDays(-i)
            };

            await userManager.CreateAsync(user, $"Password{i}!");
        }
    }
}
