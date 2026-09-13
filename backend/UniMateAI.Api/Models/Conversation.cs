namespace UniMateBackend.Models;

public class Conversation
{
    public Guid Id { get; set; }

    public string StudentId { get; set; } = "demo-student";

    public string Tool { get; set; } = string.Empty;

    public string Title { get; set; } = "New Conversation";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
}