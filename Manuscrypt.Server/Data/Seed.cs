using Manuscrypt.Server.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Manuscrypt.Server.Data
{
    public static class Seed
    {
        public static async Task SeedUsersAndPostsAsync(ManuscryptContext context, int userCount, int postsPerUser)
        {
            if (await context.Users.AnyAsync())
            {
                return;
            }

            var users = new List<User>();
            for (int i = 1; i <= userCount; i++)
            {
                var user = new User
                {
                    DisplayName = $"User{i}",
                    PhotoUrl = $"user{i}image@example.com",
                    Description = $"Description for User{i}",
                    Email = $"user{i}@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(i.ToString()),
                    CreatedAt = DateTime.UtcNow.AddDays(-i)
                };
                users.Add(user);
            }
            await context.Users.AddRangeAsync(users);
            await context.SaveChangesAsync(); // IDs are now set

            var posts = new List<Post>();
            foreach (var user in users)
            {
                for (int j = 1; j <= postsPerUser; j++)
                {
                    posts.Add(new Post
                    {
                        UserId = user.Id,
                        Title = $"Post {j} by {user.DisplayName}",
                        Description = $"This is post {j} for {user.DisplayName}.",
                        PublishedAt = DateTime.UtcNow.AddDays(-(j)),
                        Views = (user.Id * 10) + j,
                        FileUrl = "",
                        FileName = $"file_{user.Id}_{j}.txt",
                        FileType = "text/plain",
                        FileSizeBytes = 1024,
                    });
                }
            }
            await context.Posts.AddRangeAsync(posts);
            await context.SaveChangesAsync();
        }
    }
}