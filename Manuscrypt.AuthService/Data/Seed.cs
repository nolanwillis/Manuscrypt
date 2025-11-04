namespace Manuscrypt.AuthService.Data;

public class Seed
{
    public static async Task SeedUsersAsync(AuthContext context, int userCount)
    {
        //if (await context.Users.AnyAsync())
        //{
        //    return;
        //}

        //var users = new List<User>();

        //for (int i = 1; i <= userCount; i++)
        //{
        //    users.Add(new User
        //    {
        //        Id = i,
        //        DisplayName = $"User{i}",
        //        PhotoUrl = $"PhotoUrl{i}.com",
        //        Description = $"User {i}'s description.",
        //        Email = $"user{i}@somewhere.com",
        //        PasswordHash = BCrypt.Net.BCrypt.HashPassword(i.ToString()),
        //        CreatedAt = DateTime.UtcNow.AddDays(-i)
        //    });
        //}

        //await context.Users.AddRangeAsync(users);
        //await context.SaveChangesAsync();
    }
}
