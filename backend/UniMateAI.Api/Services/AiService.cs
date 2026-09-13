using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace UniMateBackend.Services;

public class AiService : IAiService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public AiService(
        HttpClient httpClient,
        IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<string> GenerateResponseAsync(
        string tool,
        string message,
        CancellationToken cancellationToken = default)
    {
        var apiKey = _configuration["Groq:ApiKey"]
                    ?? Environment.GetEnvironmentVariable("GROQ_API_KEY");
        var model = _configuration["Groq:Model"]
                    ?? "llama-3.3-70b-versatile";

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            return BuildFallbackResponse(tool, message);
        }

        var systemPrompt = GetSystemPrompt(tool);

        var requestBody = new
        {
            model,
            messages = new[]
            {
                new
                {
                    role = "system",
                    content = systemPrompt
                },
                new
                {
                    role = "user",
                    content = message
                }
            },
            temperature = 0.7,
            max_tokens = 1000
        };

        var json = JsonSerializer.Serialize(requestBody);

        try
        {
            using var request = new HttpRequestMessage(
                HttpMethod.Post,
                "https://api.groq.com/openai/v1/chat/completions");

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", apiKey);

            request.Content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json");

            using var response = await _httpClient.SendAsync(
                request,
                cancellationToken);

            var responseBody =
                await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return BuildFallbackResponse(tool, message,
                    $"Groq unavailable (HTTP {(int)response.StatusCode}).");
            }

            using var document =
                JsonDocument.Parse(responseBody);

            var content = document.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            return content ?? "Sorry, I could not generate a response.";
        }
        catch (Exception)
        {
            return BuildFallbackResponse(tool, message,
                "The AI service is temporarily unavailable.");
        }
    }

    private static string BuildFallbackResponse(
        string tool,
        string message,
        string reason = "")
    {
        var safeTool = string.IsNullOrWhiteSpace(tool) ? "chat" : tool.ToLowerInvariant();

        var intro = safeTool switch
        {
            "viva" => "Here is a quick viva-style answer for you:",
            "course-map" => "Here is the practical way to think about this course path:",
            "deadlines" => "Here is the priority guidance for your deadlines:",
            "documents" => "Here is a simple way to approach this document:",
            "career" => "Here is a practical career direction to consider:",
            "internships" => "Here is a realistic internship strategy for you:",
            "roadmap" => "Here is a focused roadmap suggestion:",
            "journey" => "Here is a realistic academic plan to consider:",
            _ => "Here is a helpful answer based on your current academic context:"
        };

        var detail = string.IsNullOrWhiteSpace(reason)
            ? "The live Groq AI connection is not configured in this local environment, so I am responding in demo mode."
            : reason;

        return $"{intro}\n\n{detail}\n\nFor: \"{message.Trim()}\"\n\nA good next step is to focus on the key requirement, break it into smaller actions, and review your course materials or academic advisor feedback before finalizing any decision.";
    }

    private static string GetSystemPrompt(string tool)
    {
        return tool.ToLowerInvariant() switch
        {
            "chat" =>
                "You are UniMate AI, a helpful university student assistant. Give clear, accurate and practical answers in simple language.",

            "viva" =>
                "You are UniMate AI Viva Coach. Help university students prepare for viva questions. Give simple explanations, likely follow-up questions, and short exam-friendly answers.",

            "course-map" =>
                "You are UniMate AI Course Map Assistant. Explain course dependencies, prerequisites and useful learning order in simple language.",

            "deadlines" =>
                "You are UniMate AI Deadline Assistant. Help students organize assignments, exams, projects and academic deadlines clearly.",

            "documents" =>
                "You are UniMate AI Document Assistant. Help students understand, summarize and prepare university-related documents in clear language.",

            "career" =>
                "You are UniMate AI Career Advisor. Give practical career guidance for university students, including skills, projects, internships and job preparation.",

            "internships" =>
                "You are UniMate AI Internship Matcher. Help students identify suitable internship areas and explain the skills they should develop.",

            "roadmap" =>
                "You are UniMate AI Roadmap Generator. Create practical step-by-step learning roadmaps based on a student's goals, current skills and available time.",

            "journey" =>
                "You are UniMate AI Academic Journey Assistant. Help students understand and plan their academic progress, courses, skills and future goals.",

            _ =>
                "You are UniMate AI, a helpful university student assistant. Give clear and practical answers in simple language."
        };
    }
}