namespace Manuscrypt.CommentService.Data;

public class Seed
{
    public static async Task SeedCommentsAsync(CommentContext context, int userCount)
    {
        foreach (var comment in context.Comments)
        {
            context.Comments.Remove(comment);
        }
        await context.SaveChangesAsync();

        var comments = new List<Comment>();
        int commentId = 1;

        // Make a comment for each user on each post.
        for (int postId = 1; postId <= userCount; postId++)
        {
            for (int userId = 1; userId <= userCount; userId++)
            {
                comments.Add(new Comment
                {
                    Id = commentId++,
                    PostId = postId,
                    UserId = userId,
                    Content = $"Comment by User {userId} on Post {postId}",
                    CreatedAt = DateTime.UtcNow.AddMinutes(-(postId + userId))
                });
            }
        }

        await context.Comments.AddRangeAsync(comments);
        await context.SaveChangesAsync();
    }
}
