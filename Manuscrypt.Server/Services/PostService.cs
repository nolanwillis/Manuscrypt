using Manuscrypt.Server.Data.Models;
using Manuscrypt.Server.Data.Repositories;
using Manuscrypt.Server.Data.DTOs;
using Manuscrypt.Server.Data;

namespace Manuscrypt.Server.Services;

public class PostService
{
    private readonly ManuscryptContext _context;
    private readonly PostRepo _postRepo;

    public PostService(ManuscryptContext context, PostRepo postRepo)
    {
        _context = context;
        _postRepo = postRepo;
    }

    public async Task<Post> CreatePostAsync(PostDTO postDto)
    {
        // Map DTO to Post entity
        var post = new Post
        {
            Title = postDto.Title,
            ChannelId = postDto.ChannelId,
            IsForChildren = postDto.IsForChildren,
            FileUrl = "",
            FileName = postDto.FileName,
            FileType = postDto.FileType,
            FileSizeBytes = postDto.FileSizeBytes,
            FileUploadedAt = postDto.FileUploadedAt,
            PublishedAt = DateTime.UtcNow
        };

        // Use PostRepo to add and save
        await _postRepo.AddAsync(post);
        await _context.SaveChangesAsync();

        return post;
    }
}
