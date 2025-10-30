using Manuscrypt.Shared;
using Manuscrypt.Shared.DTOs.User;
using Manuscrypt.UserService.Data;
using Manuscrypt.UserService.Data.Repositories;
using Manuscrypt.UserService.Exceptions;

namespace Manuscrypt.UserService;

public class UserDomainService
{
    private readonly UserRepo _userRepo;
    private readonly EventRepo _eventRepo;

    public UserDomainService(UserRepo userRepo, EventRepo eventRepo)
    {
        _userRepo = userRepo;
        _eventRepo = eventRepo;
    }

    public virtual async Task<GetUserDTO> GetUserAsync(int userId)
    {
        var user = await _userRepo.GetAsync(userId);

        if (user == null)
        {
            throw new UserDNEWithIdException(userId);
        }

        var getUserDTO = new GetUserDTO()
        {
            Id = user.Id,
            DisplayName = user.DisplayName,
            Description = user.Description,
            Email = user.Email,
            CreatedAt = user.CreatedAt,
            PhotoUrl = user.PhotoUrl
        };

        return getUserDTO;
    }
   
    public virtual async Task<IEnumerable<GetUserDTO>> GetUsersByIdsAsync(IEnumerable<int> userIds)
    {
        var users = await _userRepo.GetUsersByIdsAsync(userIds);

        var getUserDTO = users.Select(u => new GetUserDTO
        {
            Id = u.Id,
            DisplayName = u.DisplayName,
            Description = u.Description,
            Email = u.Email,
            CreatedAt = u.CreatedAt,
        });

        return getUserDTO;
    }

    public async Task<IEnumerable<Event>> GetEventsAsync(int startId, int endId)
        => await _eventRepo.GetAsync(startId, endId);

    public virtual async Task<GetUserDTO> CreateUserAsync(CreateUserDTO createUserDTO)
    {
        var existingUser = await _userRepo.GetByEmailAsync(createUserDTO.Email);

        if (existingUser != null)
        {
            throw new UserExistsException(createUserDTO.Email);
        }

        var user = new User
        {
            DisplayName = createUserDTO.DisplayName,
            Email = createUserDTO.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(createUserDTO.Password),
            CreatedAt = DateTime.UtcNow
        };

        await _userRepo.CreateAsync(user);
        await _eventRepo.AddCreateUserEvent(user);

        var getUserDTO = new GetUserDTO
        {
            Id = user.Id,
            DisplayName = user.DisplayName,
            Email = user.Email,
            CreatedAt = user.CreatedAt,
            PhotoUrl = user.PhotoUrl
        };

        return getUserDTO;
    }

    public virtual async Task UpdateUserAsync(UpdateUserDTO updateUserDTO)
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

        await _userRepo.UpdateAsync(user);
    }

    public virtual async Task DeleteUserAsync(int userId) 
    { 
        await _userRepo.DeleteAsync(userId);
        await _eventRepo.AddDeleteUserEvent(new { Id = userId });
    } 

    public virtual async Task<int> LoginAsync(LoginDTO loginDTO)
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
