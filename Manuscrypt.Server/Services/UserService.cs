using Manuscrypt.Server.Data;
using Manuscrypt.Server.Data.Models;
using Manuscrypt.Server.Data.DTOs;
using Manuscrypt.Server.Data.Repositories;
using Manuscrypt.Server.Services.Exceptions;

namespace Manuscrypt.Server.Services;

public class UserService
{
    private readonly ManuscryptContext _context;
    private readonly UserRepo _userRepo;
    private readonly ChannelRepo _channelRepo;

    public UserService(ManuscryptContext context, UserRepo userRepo, ChannelRepo channelRepo)
    {
        _context = context;
        _userRepo = userRepo;
        _channelRepo = channelRepo;
    }

    public async Task<int> CreateUser(CreateUserDTO createUserDTO)
    {
        var existingUser = await _userRepo.FindByEmailAsync(createUserDTO.Email);

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

        // Add a new channel for the new user to the DB.
        var channel = new Channel
        {
            UserId = user.Id
        };
        channel.User = user;
        user.Channel = channel;
        await _channelRepo.AddAsync(channel);

        await _context.SaveChangesAsync();

        return user.Id;
    }

    public async Task<int> Login(LoginDTO loginDTO)
    {
        var user = await _userRepo.FindByEmailAsync(loginDTO.Email);
        
        if (user == null)
        {
            throw new UserDoesNotExistException(loginDTO.Email);
        }

        if (!BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.PasswordHash))
        {
            throw new IncorrectPasswordException();
        }

        return user.Id;
    }
}
