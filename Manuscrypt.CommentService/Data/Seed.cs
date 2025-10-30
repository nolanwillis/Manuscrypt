using Microsoft.EntityFrameworkCore;

namespace Manuscrypt.CommentService.Data;

public class Seed
{
    public static async Task SeedCommentsAsync(CommentContext context, int userCount, int postCount, int commentCount)
    {
        if (await context.Comments.AnyAsync())
        {
            return;
        }

        var comments = new List<Comment>();
        int commentId = 1;

        // For each post
        for (int postId = 1; postId <= postCount; postId++)
        {
            // For each user
            for (int userId = 1; userId <= userCount; userId++)
            {
                // Each user makes 'commentCount' comments on each post
                for (int c = 1; c <= commentCount; c++)
                {
                    comments.Add(new Comment
                    {
                        Id = commentId++,
                        PostId = postId,
                        UserId = userId,
                        Content = $"Comment {c} by User {userId} on Post {postId}",
                        CreatedAt = DateTime.UtcNow.AddMinutes(-(postId + userId + c))
                    });
                }
            }
        }

        await context.Comments.AddRangeAsync(comments);
        await context.SaveChangesAsync();
    }
}
