namespace Shared;

/// <summary>
/// Single error entry, often aligned with an HTTP problem (code may repeat the HTTP status).
/// </summary>
public class Error
{
    /// <summary>Optional machine-oriented code (may match HTTP status).</summary>
    public int Code { get; set; }

    /// <summary>Human-readable error text.</summary>
    public string? Message { get; set; }
}
