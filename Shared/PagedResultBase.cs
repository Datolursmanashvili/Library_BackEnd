namespace Shared;

/// <summary>
/// Common pagination metadata for list responses.
/// </summary>
public abstract class PagedResultBase
{
    /// <summary>Total number of items matching the query (all pages).</summary>
    public int TotalCount { get; set; }

    /// <summary>Current 1-based page index returned after normalization.</summary>
    public int Page { get; set; }

    /// <summary>Page size used after normalization (clamped to allowed maximum).</summary>
    public int PageSize { get; set; }
}
