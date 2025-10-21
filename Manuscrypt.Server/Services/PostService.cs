using Manuscrypt.Server.Data;
using Manuscrypt.Server.Data.DTOs.Comment;
using Manuscrypt.Server.Data.DTOs.Post;
using Manuscrypt.Server.Data.Models;
using Manuscrypt.Server.Data.Repositories;
using Manuscrypt.Server.Services.Exceptions;

namespace Manuscrypt.Server.Services;

public class PostService
{
    private readonly ManuscryptContext _context;
    private readonly PostRepo _postRepo;
    private readonly UserRepo _userRepo;

    public PostService(ManuscryptContext context, PostRepo postRepo, UserRepo userRepo)
    {
        _context = context;
        _postRepo = postRepo;
        _userRepo = userRepo;
    }

    public async Task<GetPostDTO> GetPostAsync(int postId)
    {
        var post = await _postRepo.GetAsync(postId);
        if (post == null)
        { 
            throw new PostDoesNotExistException(postId);
        }

        var user = await _userRepo.GetAsync(post.UserId);
        if (user == null)
        {
            throw new UserDNEWithIdException(post.UserId);
        }

        var postDTO = new GetPostDTO
        {
            Id = post.Id,
            DisplayName = user.DisplayName,
            Title = post.Title,
            Description = post.Description,
            PublishedAt = DateTime.UtcNow,
            Views = post.Views,
            FileUrl = post.FileUrl
        };

        return postDTO;
    }
    public async Task<IEnumerable<GetPostDTO>> GetPostsAsync()
    {
        var posts = await _postRepo.GetAllPostsAsync();

        var userIds = posts.Select(p => p.UserId).Distinct().ToList();

        var users = await _userRepo.GetAllAsync(userIds);
       
        var userDict = users.ToDictionary(u => u.Id, u => u.DisplayName);

        var postDTOs = posts.Select(post => new GetPostDTO
        {
            Id = post.Id,
            DisplayName = userDict.TryGetValue(post.UserId, out var displayName) ? displayName : "Unknown",
            Title = post.Title,
            Description = post.Description,
            PublishedAt = post.PublishedAt,
            Views = post.Views,
            FileUrl = post.FileUrl
        }).ToList();

        return postDTOs;
    }
    public async Task<IEnumerable<GetCommentDTO>> GetCommentsForPostAsync(int postId)
    {
        var comments = await _postRepo.GetCommentsForPostAsync(postId);

        var commentDTOs = comments.Select(comment => new GetCommentDTO
        {
            Id = comment.Id,
            PostId = comment.PostId,
            UserId = comment.UserId,
            Content = comment.Content,
            CreatedAt = comment.CreatedAt
        }).ToList();

        return commentDTOs;
    }

    public async Task<int> CreatePostAsync(CreatePostDTO createPostDto)
    {
        // Add a new Post to the DB.
        var post = new Post
        {
            UserId = createPostDto.UserId,
            Title = createPostDto.Title,
            Description = createPostDto.Description,
            PublishedAt = DateTime.UtcNow,
            Views = 0,
            Tags = createPostDto.Tags,
            FileUrl = "",
            FileName = createPostDto.FileName,
            FileType = createPostDto.FileType,
            FileSizeBytes = createPostDto.FileSizeBytes,
        };

        await _postRepo.AddAsync(post);
        await _context.SaveChangesAsync();

        return post.Id;
    }

    public async Task UpdatePostAsync(UpdatePostDTO updatePostDTO)
    {
        var post = await _postRepo.GetAsync(updatePostDTO.Id);
        if (post == null)
        {
            throw new PostDoesNotExistException(updatePostDTO.Id);
        }

        post.Title = updatePostDTO.Title;
        post.Description = updatePostDTO.Description;
        post.FileUrl = updatePostDTO.FileUrl;
        _postRepo.Update(post);
        _context.SaveChanges();
    }

    public async Task DeletePostAsync(int postId)
    {
        var post = await _postRepo.GetAsync(postId);
        if (post == null)
        {
            throw new PostDoesNotExistException(postId);
        }

        _postRepo.Delete(post);
        _context.SaveChanges();
    }
}
