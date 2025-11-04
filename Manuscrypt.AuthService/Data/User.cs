using Microsoft.AspNetCore.Identity;

namespace Manuscrypt.AuthService.Data
{
    public class User : IdentityUser
    {
        public string DisplayName { get; set; } = string.Empty;
        public string PhotoUrl { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

namespace Manuscrypt.AuthService.Exceptions
{
    public class IdentityOperationException : Exception
    {
        public IdentityOperationException(string description) : base(description) {}
    }

    public class IncorrectPasswordException : Exception
    {
        public IncorrectPasswordException() : base($"Incorrect password.") {}
    }

    public class InvalidEmailException : Exception
    {
        public InvalidEmailException(string email) : base($"{email} is an invalid email.") {}
    }
    
    public class InvalidPasswordException : Exception
    {
        public InvalidPasswordException(string description) : base(description) {}
    }
    
    public class UserAlreadyExistsException : Exception
    {
        public UserAlreadyExistsException(string email)
            : base($"An user with the email {email} already exists.") {}
    }
    
    public class UserDoesNotExistWithEmailException : Exception
    {
        public UserDoesNotExistWithEmailException(string email)
            : base($"A user with the email {email} does not exist.") {}
    }
    
    public class UserDoesNotExistWithIdException : Exception
    {
        public UserDoesNotExistWithIdException(string userId)
            : base($"An user with the id {userId} does not exist.") {}
    }
}
