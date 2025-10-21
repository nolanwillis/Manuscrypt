using Manuscrypt.Server.Data;
using Manuscrypt.Server.Data.DTOs.Comment;
using Manuscrypt.Server.Data.DTOs.Post;
using Manuscrypt.Server.Data.DTOs.Subscription;
using Manuscrypt.Server.Data.DTOs.User;
using Manuscrypt.Server.Data.Models;
using Manuscrypt.Server.Data.Repositories;
using Manuscrypt.Server.Services.Exceptions;

namespace Manuscrypt.Server.Services;

public class UserService
{
    private readonly ManuscryptContext _context;
    private readonly UserRepo _userRepo;

    public UserService(ManuscryptContext context, UserRepo userRepo)
    {
        _context = context;
        _userRepo = userRepo;
    }
    
    public async Task<GetUserDTO> GetUserAsync(int userId)
    {
        var user = await _userRepo.GetAsync(userId);

        if (user == null)
        {
            throw new UserDNEWithIdException(userId);
        }

        GetUserDTO userDTO = new GetUserDTO()
        {
            Id = user.Id,
            DisplayName = user.DisplayName,
            Description = user.Description,
            Email = user.Email,
            CreatedAt = user.CreatedAt,
            PhotoUrl = user.PhotoUrl
        };

        return userDTO;
    }
    public async Task<IEnumerable<GetUserDTO>> GetUsersByIdsAsync(IEnumerable<int> userIds)
    {
        var users = await _userRepo.GetAllAsync(userIds);

        return users.Select(u => new GetUserDTO
        {
            Id = u.Id,
            DisplayName = u.DisplayName,
            Description= u.Description,
            Email = u.Email,
            CreatedAt = u.CreatedAt,
        });
    }
    public async Task<IEnumerable<GetCommentDTO>> GetCommentsByUserAsync(int userId)
    {
        var comments = await _userRepo.GetCommentsByUserAsync(userId);

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
    public async Task<IEnumerable<GetPostDTO>> GetPostsForUserAsync(int userId)
    {
        var posts = await _userRepo.GetPostsForUserAsync(userId);

        var user = await _userRepo.GetAsync(userId);
        if (user == null)
        {
            throw new UserDNEWithIdException(userId);
        }

        var postDTOs = posts.Select(post => new GetPostDTO
        {
            Id = post.Id,
            DisplayName = user.DisplayName,
            Title = post.Title,
            Description = post.Description,
            PublishedAt = post.PublishedAt,
            Views = post.Views,
            FileUrl = post.FileUrl
        }).ToList();

        return postDTOs;
    }
    public async Task<IEnumerable<GetSubscriptionDTO>> GetSubscribersForUserAsync(int userId)
    {
        var subscribers = await _userRepo.GetSubscribersForUserAsync(userId);

        var subscriptionDTOs = subscribers.Select(subscription => new GetSubscriptionDTO
        {
            Id = subscription.Id,
            SubscriberId = subscription.SubscriberId,
            SubscribedToId = subscription.SubscribedToId,
            CreatedAt = subscription.CreatedAt
        }).ToList();

        return subscriptionDTOs;
    }
    public async Task<IEnumerable<GetSubscriptionDTO>> GetSubscriptionsForUserAsync(int userId)
    {
        var subscriptions = await _userRepo.GetSubscriptionsForUserAsync(userId);

        var subscriptionDTOs = subscriptions.Select(subscription => new GetSubscriptionDTO
        {
            Id = subscription.Id,
            SubscriberId = subscription.SubscriberId,
            SubscribedToId = subscription.SubscribedToId,
            CreatedAt = subscription.CreatedAt
        }).ToList();

        return subscriptionDTOs;
    }

    public async Task<int> CreateUserAsync(CreateUserDTO createUserDTO)
    {
        var existingUser = await _userRepo.GetByEmailAsync(createUserDTO.Email);

        if (existingUser != null)
        {
            throw new UserExistsException(createUserDTO.Email);
        }
        
        // Add new user to the DB.
        var user = new User
        {
            DisplayName = createUserDTO.DisplayName,
            Email = createUserDTO.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(createUserDTO.Password),
            CreatedAt = DateTime.UtcNow
        };
        await _userRepo.AddAsync(user);
        await _context.SaveChangesAsync();

        return user.Id;
    }

    public async Task UpdateUserAsync(UpdateUserDTO updateUserDTO)
    {
        var user = await _userRepo.GetAsync(updateUserDTO.Id);
        if (user == null)
        {
            throw new UserDNEWithIdException(updateUserDTO.Id);
        }

        user.DisplayName = updateUserDTO.DisplayName;
        user.Description = updateUserDTO.Description;
        user.Email = updateUserDTO.Email;
        user.PhotoUrl = updateUserDTO.PhotoUrl;
        _userRepo.Update(user);
        _context.SaveChanges();
    }

    public async Task DeleteUserAsync(int id)
    {
        var user = await _userRepo.GetAsync(id);
        if (user == null)
        {
            throw new UserDNEWithIdException(id);
        }

        _userRepo.Delete(user);
        _context.SaveChanges();
    }

    public async Task<int> LoginAsync(LoginDTO loginDTO)
    {
        var user = await _userRepo.GetByEmailAsync(loginDTO.Email);
        
        if (user == null)
        {
            throw new UserDNEWithEmailException(loginDTO.Email);
        }

        if (!BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.PasswordHash))
        {
            throw new IncorrectPasswordException();
        }

        return user.Id;
    }
}
