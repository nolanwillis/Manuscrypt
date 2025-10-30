using Manuscrypt.PostService.Data;
using Manuscrypt.PostService.Data.Repositories;
using Manuscrypt.PostService.Exceptions;
using Manuscrypt.Shared;
using Manuscrypt.Shared.DTOs.Post;

namespace Manuscrypt.PostService;

public class PostDomainService
{
    private readonly PostRepo _postRepo;
    private readonly EventRepo _eventRepo;
    private readonly UserServiceClient _userServiceClient;

    public PostDomainService(PostRepo postRepo, EventRepo eventRepo, UserServiceClient userServiceClient)
    {
        _postRepo = postRepo;
        _eventRepo = eventRepo;
        _userServiceClient = userServiceClient;
    }

    public virtual async Task<GetPostDTO> GetPostAsync(int postId)
    {
        var post = await _postRepo.GetAsync(postId);
        if (post == null)
        {
            throw new PostDoesNotExistException(postId);
        }

        try
        {
            var user = await _userServiceClient.GetAsync(post.UserId);
            
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
        catch (HttpRequestException)
        {
            throw new UserForPostDoesNotExistException(post.UserId);
        }
    }

    public virtual async Task<IEnumerable<GetPostDTO>> GetPostsByUserAsync(int userId)
    {
        try
        {
            var user = await _userServiceClient.GetAsync(userId);
            var posts = await _postRepo.GetPostsByUserAsync(userId);

            var getPostDTOS = posts.Select(post => new GetPostDTO
            {
                Id = post.Id,
                DisplayName = user.DisplayName,
                Title = post.Title,
                Description = post.Description,
                PublishedAt = post.PublishedAt,
                Views = post.Views,
                FileUrl = post.FileUrl
            });

            return getPostDTOS;
        }
        catch (HttpRequestException)
        {
            throw new UserForPostDoesNotExistException(userId);
        }
    }

    public async Task<IEnumerable<Event>> GetEventsAsync(int startId, int endId)
      => await _eventRepo.GetAsync(startId, endId);

    public virtual async Task<GetPostDTO> CreatePostAsync(CreatePostDTO createPostDTO)
    {
        try
        {
            var user = await _userServiceClient.GetAsync(createPostDTO.UserId);
            
            var post = new Post
            {
                UserId = createPostDTO.UserId,
                Title = createPostDTO.Title,
                Description = createPostDTO.Description,
                PublishedAt = DateTime.UtcNow,
                Views = 0,
                Tags = createPostDTO.Tags,
                FileUrl = "",
                FileName = createPostDTO.FileName,
                FileType = createPostDTO.FileType,
                FileSizeBytes = createPostDTO.FileSizeBytes,
            };

            await _postRepo.AddAsync(post);
            await _eventRepo.AddCreatePostEventAsync(post);

            var getPostDTO = new GetPostDTO
            {
                Id = post.Id,
                DisplayName = user.DisplayName,
                Title = post.Title,
                Description = post.Description,
                PublishedAt = post.PublishedAt,
                Views = post.Views,
                FileUrl = post.FileUrl
            };

            return getPostDTO;
        }
        catch (HttpRequestException)
        {
            throw new UserForPostDoesNotExistException(createPostDTO.UserId);
        }
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

    public virtual async Task DeletePostAsync(int postId)
    {
        await _postRepo.DeleteAsync(postId);
        await _eventRepo.AddDeletePostEventAsync( new { Id = postId });
    } 
}
