namespace Manuscrypt.Shared;

public class Event
{
    public int Id { get; set; }
    public EventType Type { get; set; }
    public DateTime OccuredAt { get; set; }
    public string ContentJson { get; set; }

    public void Print()
        => WriteLine($"[id: {Id}, type: {Type.ToString()}, occured-at: {OccuredAt.ToString()}]");
}

public enum EventType
{
    CreateComment,
    CreatePost,
    CreateSubscription,
    CreateUser,

    DeleteComment,
    DeletePost,
    DeleteSubscription,
    DeleteUser
}
