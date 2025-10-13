namespace Manuscrypt.Server.Data.Models
{
    public class User
    {
        public int Id { get; set; }
        public string DisplayName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool IsChild { get; set; } = false;

        // Navigation
        public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Edit> Edits { get; set; } = new List<Edit>();
    }
}

namespace Manuscrypt.Server.Services.Exceptions
{
    public class UserDoesNotExistException : Exception
    {
        public UserDoesNotExistException(string email)
            : base($"An account with the email {email} does not exist.") { }
    }
}
