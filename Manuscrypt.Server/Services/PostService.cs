using Manuscrypt.Server.Data.DTOs.Comment;
using Manuscrypt.Server.Data.DTOs.Post;
using Manuscrypt.Server.Data.Models;
using Manuscrypt.Server.Data.Repositories;
using Manuscrypt.Server.Services.Exceptions;

namespace Manuscrypt.Server.Services;

public class PostService
{
    private readonly PostRepo _postRepo;
    private readonly UserRepo _userRepo;

    public PostService(PostRepo postRepo, UserRepo userRepo)
    {
        _postRepo = postRepo;
        _userRepo = userRepo;
    }

    public virtual async Task<GetPostDTO> GetPostAsync(int postId)
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
    public virtual async Task<IEnumerable<GetCommentDTO>> GetCommentsForPostAsync(int postId)
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

    public virtual async Task<int> CreatePostAsync(CreatePostDTO createPostDto)
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

        return post.Id;
    }

    public virtual async Task UpdatePostAsync(UpdatePostDTO updatePostDTO)
    {
        var post = await _postRepo.GetAsync(updatePostDTO.Id);
        if (post == null)
        {
            throw new PostDoesNotExistException(updatePostDTO.Id);
        }

        post.Title = updatePostDTO.Title;
        post.Description = updatePostDTO.Description;
        post.FileUrl = updatePostDTO.FileUrl;

        await _postRepo.UpdateAsync(post);
    }

    public virtual async Task DeletePostAsync(int postId) => await _postRepo.DeleteAsync(postId);
}
