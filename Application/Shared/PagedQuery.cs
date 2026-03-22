namespace Application.Shared;

/// <summary>
/// Base for list queries with 1-based <see cref="Page"/> and bounded <see cref="PageSize"/>.
/// </summary>
/// <typeparam name="TResult">Result type wrapped in <see cref="Query{TQueryResult}"/> execution.</typeparam>
public abstract class PagedQuery<TResult> : Query<TResult> where TResult : class
{
    /// <summary>1-based page index (default 1).</summary>
    public int Page { get; set; } = 1;

    /// <summary>Items per page (default from <see cref="Pagination.DefaultPageSize"/>, max <see cref="Pagination.MaxPageSize"/>).</summary>
    public int PageSize { get; set; } = Pagination.DefaultPageSize;

    /// <summary>Computes skip/take and normalized page values for EF queries.</summary>
    /// <returns>Tuple: skip, take, normalized page, normalized page size.</returns>
    protected (int Skip, int Take, int Page, int PageSize) GetNormalizedPaging()
    {
        var page = Page < 1 ? 1 : Page;
        var size = PageSize < 1 ? Pagination.DefaultPageSize : PageSize;
        if (size > Pagination.MaxPageSize)
            size = Pagination.MaxPageSize;
        return ((page - 1) * size, size, page, size);
    }
}
