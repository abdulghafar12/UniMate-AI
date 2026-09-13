namespace UniMateBackend.DTOs;

public class CreateConversationRequest
{
    public string Tool { get; set; } = string.Empty;

    public string StudentId { get; set; } = "demo-student";

    public string? Title { get; set; }
}

public class SendMessageRequest
{
    public Guid? ConversationId { get; set; }

    public string Tool { get; set; } = string.Empty;

    public string StudentId { get; set; } = "demo-student";

    public string Message { get; set; } = string.Empty;
}

public class ConversationSummaryDto
{
    public Guid Id { get; set; }

    public string Tool { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}

public class MessageDto
{
    public Guid Id { get; set; }

    public string Role { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}

public class ConversationDto
{
    public Guid Id { get; set; }

    public string StudentId { get; set; } = string.Empty;

    public string Tool { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public List<MessageDto> Messages { get; set; } = new();
}

public class SendMessageResponseDto
{
    public ConversationDto Conversation { get; set; } = new();

    public MessageDto UserMessage { get; set; } = new();

    public MessageDto AssistantMessage { get; set; } = new();
}