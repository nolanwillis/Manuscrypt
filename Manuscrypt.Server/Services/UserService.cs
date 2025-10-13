using Manuscrypt.Server.Data;
using Manuscrypt.Server.Data.DTOs;
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

    public async Task<UserDTO> Login(string email)
    {
        var user = await _userRepo.FindByEmailAsync(email);
        
        if (user == null)
        {
            throw new UserDoesNotExistException(email);
        }

        var UserDTO = new UserDTO
        {
            Id = user.Id,
            DisplayName = user.DisplayName,
            Email = user.Email,
            CreatedAt = user.CreatedAt,
            IsChild = user.IsChild
        };

        return UserDTO;
    }
}
