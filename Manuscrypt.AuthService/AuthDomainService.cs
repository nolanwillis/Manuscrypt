using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Manuscrypt.Shared;
using Manuscrypt.Shared.DTOs.User;
using Manuscrypt.AuthService.Data;
using Manuscrypt.AuthService.Data.Repositories;
using Manuscrypt.AuthService.Exceptions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Manuscrypt.AuthService;

public class AuthDomainService
{
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _config;
    private readonly EventRepo _eventRepo;

    public AuthDomainService(UserManager<User> userManager, IConfiguration config, EventRepo eventRepo)
    {
        _userManager = userManager;
        _config = config;
        _eventRepo = eventRepo;
    }

    public virtual async Task<GetUserDTO> GetAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            throw new UserDoesNotExistWithIdException(userId);
        }

        var getUserDTO = new GetUserDTO()
        {
            Id = user.Id,
            DisplayName = user.DisplayName,
            Description = user.Description,
            Email = user.Email ?? "",
            CreatedAt = user.CreatedAt,
            PhotoUrl = user.PhotoUrl
        };

        return getUserDTO;
    }

    public virtual async Task<IEnumerable<GetUserDTO>> GetUsersByIdsAsync(IEnumerable<string> userIds)
    {
        var users = await _userManager.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();

        var getUserDTO = users.Select(u => new GetUserDTO
        {
            Id = u.Id,
            DisplayName = u.DisplayName,
            Description = u.Description,
            Email = u.Email ?? "",
            CreatedAt = u.CreatedAt,
        });

        return getUserDTO;
    }
    
    public virtual async Task<GetUserDTO?> GetBySeedIdAsync(int seedId)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.SeedId == seedId);
        if (user == null)
        {
            return null;
        }

        var getUserDTO = new GetUserDTO
        {
            Id = user.Id,
            DisplayName = user.DisplayName,
            Description = user.Description,
            Email = user.Email ?? string.Empty,
            CreatedAt = user.CreatedAt,
            PhotoUrl = user.PhotoUrl
        };
        
        return getUserDTO;
    }

    public async Task<IEnumerable<Event>> GetEventsAsync(int startId, int endId)
        => await _eventRepo.GetAsync(startId, endId);

    public virtual async Task<object> CreateUserAsync(CreateUserDTO createUserDTO)
    {
        var user = new User { Email = createUserDTO.Email, UserName = createUserDTO.DisplayName };

        var result = await _userManager.CreateAsync(user, createUserDTO.Password);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                switch (error.Code)
                {
                    case "DuplicateUserName":
                        throw new UserAlreadyExistsException(user.UserName);
                    case "DuplicateEmail":
                        throw new UserAlreadyExistsException(user.Email);
                    case "InvalidEmail":
                        throw new InvalidEmailException(user.Email);
                    case "PasswordTooShort":
                    case "PasswordRequiresNonAlphanumeric":
                    case "PasswordRequiresDigit":
                    case "PasswordRequiresUpper":
                    case "PasswordRequiresLower":
                        throw new InvalidPasswordException(error.Description);
                    // Add more cases as needed
                    default:
                        throw new IdentityOperationException(error.Description);
                }
            }
        }

        await _eventRepo.AddCreateUserEvent(user);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? "")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(issuer: _config["Jwt:Issuer"], claims: claims,
            expires: DateTime.UtcNow.AddHours(1), signingCredentials: creds);

        return new { token = new JwtSecurityTokenHandler().WriteToken(token) };
    }

    public virtual async Task UpdateUserAsync(UpdateUserDTO updateUserDTO)
    {
        var user = await _userManager.FindByIdAsync(updateUserDTO.Id);
        if (user == null)
        {
            throw new UserDoesNotExistWithIdException(updateUserDTO.Id);
        } 

        user.DisplayName = updateUserDTO.DisplayName ?? user.DisplayName;
        user.Description = updateUserDTO.Description ?? user.Description;
        user.Email = updateUserDTO.Email ?? user.Email;
        user.PhotoUrl = updateUserDTO.PhotoUrl ?? user.PhotoUrl;

        await _userManager.UpdateAsync(user);
    }

    public virtual async Task DeleteUserAsync(string userId) 
    { 
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new UserDoesNotExistWithIdException(userId);
        }
        
        await _userManager.DeleteAsync(user);
        await _eventRepo.AddDeleteUserEvent(new { Id = userId });
    } 

    public virtual async Task<object> LoginAsync(LoginDTO loginDTO)
    {
        var user = await _userManager.FindByEmailAsync(loginDTO.Email);
        if (user == null)
        {
            throw new UserDoesNotExistWithEmailException(loginDTO.Email);
        }
        else if (!await _userManager.CheckPasswordAsync(user, loginDTO.Password))
        {
            throw new IncorrectPasswordException();
        }

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? "")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(issuer: _config["Jwt:Issuer"], claims: claims,
            expires: DateTime.UtcNow.AddHours(1), signingCredentials: creds);

        return new { token = new JwtSecurityTokenHandler().WriteToken(token) };
    }
}
