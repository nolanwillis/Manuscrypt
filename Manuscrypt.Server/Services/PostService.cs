using Manuscrypt.Server.Data;
using Manuscrypt.Server.Data.DTOs;
using Manuscrypt.Server.Data.Models;
using Manuscrypt.Server.Data.Repositories;
using Manuscrypt.Server.Services.Exceptions;

namespace Manuscrypt.Server.Services;

public class PostService
{
    private readonly ManuscryptContext _context;
    private readonly PostRepo _postRepo;
    private readonly ChannelRepo _channelRepo;

    public PostService(ManuscryptContext context, PostRepo postRepo, ChannelRepo channelRepo)
    {
        _context = context;
        _postRepo = postRepo;
        _channelRepo = channelRepo;
    }

    public async Task<int> CreatePostAsync(CreatePostDTO createPostDto)
    {
        var channel = await _channelRepo.FindByUserIdAsync(createPostDto.UserId);

        if (channel == null)
        {
            throw new ChannelDoesNotExistException(createPostDto.UserId);
        }

        // Add a new Post to the DB.
        var post = new Post
        {
            ChannelId = channel.Id,
            Title = createPostDto.Title,
            Description = createPostDto.Description,
            PublishedAt = DateTime.UtcNow,
            Views = 0,
            FileUrl = createPostDto.FileUrl,
            FileName = createPostDto.FileName,
            FileType = createPostDto.FileType,
            FileSizeBytes = createPostDto.FileSizeBytes,
            Channel = channel
        };
        await _postRepo.AddAsync(post);
        await _context.SaveChangesAsync();

        return post.Id;
    }
}
