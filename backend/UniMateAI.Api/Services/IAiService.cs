namespace UniMateBackend.Services;

public interface IAiService
{
    Task<string> GenerateResponseAsync(
        string tool,
        string message,
        CancellationToken cancellationToken = default);
}