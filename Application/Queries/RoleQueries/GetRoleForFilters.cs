using Application.Shared;
using Domain.Entities.RoleEntity;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Application.Queries.RoleQueries;

public class GetRoleForFiltersQuery : PagedQuery<GetRoleForFiltersQueryResult>
{
    public override async Task<QueryExecutionResult<GetRoleForFiltersQueryResult>> Execute()
    {
        var (skip, take, page, pageSize) = GetNormalizedPaging();

        var query = _appContext.Roles.AsNoTracking().Where(x => !x.IsDeleted).OrderBy(x => x.Name);

        var totalCount = await query.CountAsync();

        if (totalCount == 0)
            return await Fail(StatusCodes.Status404NotFound, "თქვენ არ გაქვთ რეგისტრირებული როლი");

        var applicationRoles = await query
            .Skip(skip)
            .Take(take)
            .ToListAsync();

        var result = applicationRoles
            .Select(x => new GetRoleForFiltersResultItem
            {
                RolePermissions = x.Permissions,
                Id = x.Id,
                Name = x.Name,
            })
            .ToList();

        return await Ok(new GetRoleForFiltersQueryResult
        {
            Response = result,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
        });
    }
}

public class GetRoleForFiltersQueryValidator : AbstractValidator<GetRoleForFiltersQuery>
{
    public GetRoleForFiltersQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize).InclusiveBetween(1, Pagination.MaxPageSize);
    }
}

public class GetRoleForFiltersQueryResult : PagedResultBase
{
    public List<GetRoleForFiltersResultItem>? Response { get; set; }
}

public class GetRoleForFiltersResultItem
{
    public IEnumerable<Permissions>? RolePermissions { get; set; }
    public string Name { get; set; }
    public string Id { get; set; }
}
