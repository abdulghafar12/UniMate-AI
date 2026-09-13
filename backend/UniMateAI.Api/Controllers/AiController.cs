using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniMateBackend.Data;
using UniMateBackend.DTOs;
using UniMateBackend.Models;
using UniMateBackend.Services;

namespace UniMateBackend.Controllers;

[ApiController]
[Route("api/ai")]
public class AiController : ControllerBase
{
    private readonly UniMateDbContext _db;
    private readonly IAiService _aiService;

    private static readonly HashSet<string> ValidTools =
    new(StringComparer.OrdinalIgnoreCase)
    {
        "chat",
        "viva",
        "course-map",
        "deadlines",
        "documents",
        "career",
        "internships",
        "roadmap",
        "journey"
    };

    public AiController(
        UniMateDbContext db,
        IAiService aiService)
    {
        _db = db;
        _aiService = aiService;
    }

    [HttpGet("tools")]
    public IActionResult GetTools()
    {
        return Ok(ValidTools);
    }

    [HttpGet("conversations/{tool}")]
    public async Task<IActionResult> GetConversations(
        string tool,
        [FromQuery] string studentId = "demo-student")
    {
        if (!IsValidTool(tool))
            return BadRequest(new { message = "Invalid UniMate tool." });

        var conversations = await _db.Conversations
            .AsNoTracking()
            .Where(c =>
                c.StudentId == studentId &&
                c.Tool == tool)
            .OrderByDescending(c => c.UpdatedAt)
            .Select(c => new ConversationSummaryDto
            {
                Id = c.Id,
                Tool = c.Tool,
                Title = c.Title,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            })
            .ToListAsync();

        return Ok(conversations);
    }

    [HttpGet("conversations/{id:guid}")]
    public async Task<IActionResult> GetConversation(
        Guid id,
        [FromQuery] string studentId = "demo-student")
    {
        var conversation = await _db.Conversations
            .AsNoTracking()
            .Include(c => c.Messages)
            .FirstOrDefaultAsync(c =>
                c.Id == id &&
                c.StudentId == studentId);

        if (conversation == null)
            return NotFound(new { message = "Conversation not found." });

        return Ok(ToConversationDto(conversation));
    }

    [HttpPost("conversations")]
    public async Task<IActionResult> CreateConversation(
        [FromBody] CreateConversationRequest request)
    {
        if (!IsValidTool(request.Tool))
            return BadRequest(new { message = "Invalid UniMate tool." });

        var conversation = new Conversation
        {
            Id = Guid.NewGuid(),
            StudentId = request.StudentId,
            Tool = request.Tool.ToLowerInvariant(),
            Title = string.IsNullOrWhiteSpace(request.Title)
                ? "New Conversation"
                : request.Title.Trim(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _db.Conversations.Add(conversation);

        await _db.SaveChangesAsync();

        return Ok(ToConversationDto(conversation));
    }

    [HttpPost("messages")]
    public async Task<IActionResult> SendMessage(
        [FromBody] SendMessageRequest request,
        CancellationToken cancellationToken)
    {
        if (!IsValidTool(request.Tool))
            return BadRequest(new { message = "Invalid UniMate tool." });

        if (string.IsNullOrWhiteSpace(request.Message))
            return BadRequest(new { message = "Message cannot be empty." });

        Conversation? conversation = null;

        if (request.ConversationId.HasValue)
        {
            conversation = await _db.Conversations
                .Include(c => c.Messages)
                .FirstOrDefaultAsync(
                    c =>
                        c.Id == request.ConversationId.Value &&
                        c.StudentId == request.StudentId,
                    cancellationToken);

            if (conversation == null)
            {
                return NotFound(new
                {
                    message = "Conversation not found."
                });
            }

            if (!conversation.Tool.Equals(
                    request.Tool,
                    StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(new
                {
                    message = "Conversation belongs to another tool."
                });
            }
        }
        else
        {
            conversation = new Conversation
            {
                Id = Guid.NewGuid(),
                StudentId = request.StudentId,
                Tool = request.Tool.ToLowerInvariant(),
                Title = CreateTitle(request.Message),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _db.Conversations.Add(conversation);
        }

        if (conversation.Title == "New Conversation")
        {
            conversation.Title = CreateTitle(request.Message);
        }

        var userMessage = new ChatMessage
        {
            Id = Guid.NewGuid(),
            ConversationId = conversation.Id,
            Role = "user",
            Content = request.Message.Trim(),
            CreatedAt = DateTime.UtcNow
        };

        _db.Messages.Add(userMessage);

        string aiResponse;

        try
        {
            aiResponse = await _aiService.GenerateResponseAsync(
                request.Tool,
                request.Message,
                cancellationToken);
        }
        catch (Exception exception) when (
            exception is HttpRequestException ||
            exception is InvalidOperationException)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new
            {
                message = "UniMate AI is temporarily unavailable. Please try again."
            });
        }

        var assistantMessage = new ChatMessage
        {
            Id = Guid.NewGuid(),
            ConversationId = conversation.Id,
            Role = "assistant",
            Content = aiResponse,
            CreatedAt = DateTime.UtcNow
        };

        _db.Messages.Add(assistantMessage);

        conversation.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync(cancellationToken);

        return Ok(new SendMessageResponseDto
        {
            Conversation = ToConversationDto(conversation),
            UserMessage = ToMessageDto(userMessage),
            AssistantMessage = ToMessageDto(assistantMessage)
        });
    }

    [HttpDelete("conversations/{id:guid}")]
    public async Task<IActionResult> DeleteConversation(
        Guid id,
        [FromQuery] string studentId = "demo-student")
    {
        var conversation = await _db.Conversations
            .FirstOrDefaultAsync(c =>
                c.Id == id &&
                c.StudentId == studentId);

        if (conversation == null)
            return NotFound(new
            {
                message = "Conversation not found."
            });

        _db.Conversations.Remove(conversation);

        await _db.SaveChangesAsync();

        return Ok(new
        {
            message = "Conversation deleted successfully."
        });
    }

    private static bool IsValidTool(string tool)
    {
        return !string.IsNullOrWhiteSpace(tool)
               && ValidTools.Contains(tool);
    }

    private static string CreateTitle(string message)
    {
        var title = message.Trim();

        if (title.Length > 60)
            title = title[..60] + "...";

        return title;
    }

    private static ConversationDto ToConversationDto(
        Conversation conversation)
    {
        return new ConversationDto
        {
            Id = conversation.Id,
            StudentId = conversation.StudentId,
            Tool = conversation.Tool,
            Title = conversation.Title,
            CreatedAt = conversation.CreatedAt,
            UpdatedAt = conversation.UpdatedAt,
            Messages = conversation.Messages
                .OrderBy(m => m.CreatedAt)
                .Select(ToMessageDto)
                .ToList()
        };
    }

    private static MessageDto ToMessageDto(
        ChatMessage message)
    {
        return new MessageDto
        {
            Id = message.Id,
            Role = message.Role,
            Content = message.Content,
            CreatedAt = message.CreatedAt
        };
    }
}
