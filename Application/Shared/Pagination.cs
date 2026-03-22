namespace Application.Shared;

/// <summary>
/// Default limits for paged list queries (see <c>PagedQuery&lt;TResult&gt;</c>).
/// </summary>
public static class Pagination
{
    /// <summary>Fallback page size when the client sends an invalid value.</summary>
    public const int DefaultPageSize = 10;

    /// <summary>Maximum allowed <c>PageSize</c> to protect the API.</summary>
    public const int MaxPageSize = 100;
}
