namespace Shared;

/// <summary>
/// Result for simple command/repository operations without a typed payload. <see cref="Code"/> doubles as HTTP status hint when surfaced through the API.
/// </summary>
public class RepositoryExecutionResult
{
    /// <summary>Optional identifier produced by the operation (e.g. new entity id).</summary>
    public string? ResultId { get; set; }

    /// <summary>Optional file or resource URL.</summary>
    public string? FileUrl { get; set; }

    /// <summary>True when the operation succeeded.</summary>
    public bool Success { get; set; }

    /// <summary>Structured errors when <see cref="Success"/> is false.</summary>
    public IEnumerable<Error>? Errors { get; set; }

    /// <summary>Primary error message when <see cref="Success"/> is false.</summary>
    public string? ErrorMessage { get; set; }

    /// <summary>Optional list count for list-oriented operations.</summary>
    public long? ListCount { get; set; }

    /// <summary>HTTP status hint (defaults to 200; set to 4xx/5xx on failure).</summary>
    public int Code = 200;

    /// <summary>Optional binary payload (e.g. file bytes).</summary>
    public byte[]? Data { get; set; }
}
