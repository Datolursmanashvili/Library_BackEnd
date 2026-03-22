namespace Shared;

/// <summary>
/// Standard envelope for command operations that return a typed payload. HTTP status on the wire matches <see cref="HttpStatusCode"/> when returned via API helpers.
/// </summary>
/// <typeparam name="T">Payload type in <see cref="Data"/> on success.</typeparam>
public class CommandExecutionResultGeneric<T>
{
    /// <summary>True when the command completed without business/validation errors.</summary>
    public bool Success { get; set; }

    /// <summary>HTTP status for this result (e.g. 200, 201, 400, 409, 500).</summary>
    public int HttpStatusCode { get; set; } = 200;

    /// <summary>Errors when <see cref="Success"/> is false.</summary>
    public IEnumerable<Error> Errors { get; set; }

    /// <summary>Command result payload when <see cref="Success"/> is true.</summary>
    public T Data { get; set; }
}
