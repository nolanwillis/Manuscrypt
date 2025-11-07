using Microsoft.EntityFrameworkCore;

namespace Manuscrypt.PostService.Data;

public class Seed
{
    public static async Task SeedPostsAsync(PostContext context, int userCount)
    {
        foreach (var post in context.Posts)
        {
            context.Posts.Remove(post);
        }
        await context.SaveChangesAsync();

        var posts = new List<Post>();

        for (int i = 1; i <= userCount; i++)
        {
            posts.Add(new Post
            {
                Id = i,
                UserId = i,
                Title = $"Post {i} by user {i}",
                Description = $"This is post {i} for user {i}.",
                PublishedAt = DateTime.UtcNow.AddDays(-(i + i)),
                Views = 0,
                FileUrl = $"https://files.example.com/user{i}/post{i}.jpg",
                FileName = $"post{i}_user{i}.jpg",
                FileType = "image/jpeg",
                FileSizeBytes = 1024 * i
            });
        }

        await context.Posts.AddRangeAsync(posts);
        await context.SaveChangesAsync();
    }
}
