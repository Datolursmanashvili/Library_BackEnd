using Application.Shared;
using Domain.Entities.RoleEntity;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Application.Queries.RoleQueries;

public class GetRolesQuery : PagedQuery<GetRolesQueryResult>
{
    public string? Purpose { get; set; }

    public override async Task<QueryExecutionResult<GetRolesQueryResult>> Execute()
    {
        var (skip, take, page, pageSize) = GetNormalizedPaging();

        var query = ApplicationContext.Set<ApplicationRole>()
            .AsNoTracking()
            .Where(x => !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(Purpose))
            query = query.Where(x => x.Name != null && x.Name.Contains(Purpose));

        query = query.OrderByDescending(x => x.CreatedAt);

        var totalCount = await query.CountAsync();

        var applicationRoles = await query
            .Skip(skip)
            .Take(take)
            .ToListAsync();

        var result = applicationRoles
            .Select(x => new GetRolesQueryResultItem
            {
                Id = x.Id,
                Name = x.Name,
                Permissions = x.Permissions,
            })
            .ToList();

        return await Ok(new GetRolesQueryResult
        {
            Response = result,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
        });
    }
}

public class GetRolesQueryValidator : AbstractValidator<GetRolesQuery>
{
    public GetRolesQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize).InclusiveBetween(1, Pagination.MaxPageSize);
    }
}

public class GetRolesQueryResult : PagedResultBase
{
    public List<GetRolesQueryResultItem> Response { get; set; } = [];
}

public class GetRolesQueryResultItem
{
    public string Name { get; set; }
    public string Id { get; set; }
    public IEnumerable<Permissions>? Permissions { get; set; }
}
