using Application.Shared;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Application.Queries.RoleQueries;

/// <summary>
/// Paginated list of roles (non-deleted). Query string: <c>Page</c>, <c>PageSize</c>.
/// </summary>
public class GetAllRolesQuery : PagedQuery<GetAllRolesQueryResult>
{
    /// <inheritdoc />
    public override async Task<QueryExecutionResult<GetAllRolesQueryResult>> Execute()
    {
        var (skip, take, page, pageSize) = GetNormalizedPaging();

        var baseQuery = _appContext.Roles
            .AsNoTracking()
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.Name);

        var totalCount = await baseQuery.CountAsync();

        var items = await baseQuery
            .Skip(skip)
            .Take(take)
            .Select(x => new RoleItemResponseItem
            {
                Id = x.Id,
                Name = x.Name,
                NormaliseName = x.NormalizedName,
            })
            .ToListAsync();

        return await Ok(new GetAllRolesQueryResult
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
        });
    }
}

/// <summary>FluentValidation for <see cref="GetAllRolesQuery"/>.</summary>
public class GetAllRolesQueryValidator : AbstractValidator<GetAllRolesQuery>
{
    /// <summary>Creates the validator.</summary>
    public GetAllRolesQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize).InclusiveBetween(1, Pagination.MaxPageSize);
    }
}

/// <summary>Role summary row.</summary>
public class RoleItemResponseItem
{
    /// <summary>Role id.</summary>
    public string Id { get; set; }

    /// <summary>Display name.</summary>
    public string Name { get; set; }

    /// <summary>Normalized name (uppercase).</summary>
    public string NormaliseName { get; set; }
}

/// <summary>Paginated roles response.</summary>
public class GetAllRolesQueryResult : PagedResultBase
{
    /// <summary>Roles for the current page.</summary>
    public List<RoleItemResponseItem> Items { get; set; } = [];
}
