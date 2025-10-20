namespace Manuscrypt.Server.Data.Models
{
    public class User
    {
        public int Id { get; set; }
        public string DisplayName { get; set; } = string.Empty;
        public string? PhotoUrl { get; set; }
        public string? Description { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        // Navigation
        public ICollection<Post> posts { get; set; } = new List<Post>();
        public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
        public ICollection<Subscription> Subscribers { get; set; } = new List<Subscription>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}

namespace Manuscrypt.Server.Services.Exceptions
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
