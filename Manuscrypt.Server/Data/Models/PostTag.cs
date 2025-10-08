namespace Manuscrypt.Server.Data.Models;

public class PostTag
{
    public int Id { get; set; }
    public int PostId { get; set; }
    public Tag Tag { get; set; }

    // Navigation
    public Post Post { get; set; } = null!;
}

public enum Tag
{
    Fiction,
    Fantasy,
    Syfy,
    Nonfiction,
    NSFW,
    Romance,
    Smut,
    Chapter,
    Page,
    Poem,
    Short,
    Long,
    Scary,
    Dark,
    Raunchy,
    Sensual,
    Funny,
    Heartwarming,
    Lighthearted,
    Political,
    Disturbing,
    Informative
}
