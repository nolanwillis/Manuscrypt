using Microsoft.EntityFrameworkCore;

namespace Manuscrypt.PostService.Data;

public class Seed
{
    public static async Task SeedPostsAsync(PostContext context, int userCount, int postsPerUser)
    {
        if (await context.Posts.AnyAsync())
        {
            return;
        }

        var posts = new List<Post>();
        int postId = 1;

        for (int userId = 1; userId <= userCount; userId++)
        {
            for (int i = 1; i <= postsPerUser; i++)
            {
                posts.Add(new Post
                {
                    Id = postId++,
                    UserId = userId,
                    Title = $"Post {i} by User {userId}",
                    Description = $"This is post {i} for user {userId}.",
                    PublishedAt = DateTime.UtcNow.AddDays(-(userId + i)),
                    Views = 0,
                    FileUrl = $"https://files.example.com/user{userId}/post{i}.jpg",
                    FileName = $"post{i}_user{userId}.jpg",
                    FileType = "image/jpeg",
                    FileSizeBytes = 1024 * i
                });
            }
        }

        await context.Posts.AddRangeAsync(posts);
        await context.SaveChangesAsync();
    }
}
