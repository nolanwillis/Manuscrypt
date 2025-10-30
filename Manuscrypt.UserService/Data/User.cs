namespace Manuscrypt.UserService.Data
{
    public class User
    {
        public int Id { get; set; }
        public string DisplayName { get; set; } = string.Empty;
        public string PhotoUrl { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}

namespace Manuscrypt.UserService.Exceptions
{
    public class UserDNEWithEmailException : Exception
    {
        public UserDNEWithEmailException(string email)
            : base($"A user with the email {email} does not exist.") { }
    }

    public class UserDNEWithIdException : Exception
    {
        public UserDNEWithIdException(int userId)
            : base($"An user with the id {userId} does not exist.") { }
    }

    public class UserExistsException : Exception
    {
        public UserExistsException(string email)
            : base($"An user with the email {email} already exists.") { }
    }

    public class IncorrectPasswordException : Exception
    {
        public IncorrectPasswordException()
            : base($"Incorrect password.") { }
    }
}
