namespace Shared;

public class CommandExecutionResult
{
    public string? ResultId { get; set; }
    public string? FileUrl { get; set; }
    public bool Success { get; set; }
    public IEnumerable<Error>? Errors { get; set; }
    public string? ErrorMessage { get; set; }
    public long? ListCount { get; set; }

    public int Code = 200;
    public byte[]? Data { get; set; }
}

