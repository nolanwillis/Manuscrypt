using Manuscrypt.Server.Data;
using Manuscrypt.Server.Data.DTOs;
using Manuscrypt.Server.Data.Models;
using Manuscrypt.Server.Data.Repositories;

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

    public async Task<PostDTO> CreatePostAsync(PostDTO postDto)
    {
        
        // Map DTO to Post entity.
        var post = new Post
        {
            ChannelId = postDto.ChannelId,
            Title = postDto.Title,
            PublishedAt = DateTime.UtcNow,
            Views = 0,
            IsForChildren = postDto.IsForChildren,
            FileUrl = postDto.FileUrl,
            FileName = postDto.FileName,
            FileType = postDto.FileType,
            FileSizeBytes = postDto.FileSizeBytes,
        };
        

        // Use PostRepo to add and save to the database.
        await _postRepo.AddAsync(post);
        await _context.SaveChangesAsync();
        
        // Create the return DTO.
        var associatedChannel = await _channelRepo.FindByIdAsync(postDto.ChannelId);
        var postDTO = new PostDTO
        {
            Id = post.Id,
            ChannelId = post.ChannelId,
            ChannelName = associatedChannel?.Name ?? "",
            Title = post.Title,
            PublishedAt = post.PublishedAt,
            Views = post.Views,
            IsForChildren = post.IsForChildren 
        };

        return postDTO;
    }
}
