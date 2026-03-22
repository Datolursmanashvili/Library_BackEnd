namespace Shared;

public sealed class FileDownloadPayload
{
    public required byte[] Content { get; init; }
    public required string DownloadFileName { get; init; }
    public string ContentType { get; init; } = "application/octet-stream";
}
