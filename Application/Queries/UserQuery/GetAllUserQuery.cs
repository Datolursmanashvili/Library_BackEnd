using Application.Shared;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Application.Queries.UserQuery;

/// <summary>
/// Paginated list of application users. Inherits paging: 1-based <c>Page</c> and <c>PageSize</c> query parameters.
/// </summary>
public class GetAllUserQuery : PagedQuery<GetAllUserQueryResult>
{
    /// <inheritdoc />
    public override async Task<QueryExecutionResult<GetAllUserQueryResult>> Execute()
    {
        var (skip, take, page, pageSize) = GetNormalizedPaging();

        var baseQuery = _appContext.Users.AsNoTracking().OrderBy(x => x.UserName);
        var totalCount = await baseQuery.CountAsync();

        var pageEntities = await baseQuery
            .Skip(skip)
            .Take(take)
            .ToListAsync();

        var result = pageEntities.Select(x => new UserQueryResultItem
        {
            Id = x.Id,
            Email = x.Email,
            FirstName = x.FirstName,
            LastName = x.LastName,
            PNumber = userRepository.Decrypt(x.PNumber),
            Phone = x.PhoneNumber,
            Username = x.UserName,
            DepartmentId = x.DepartmentId,
            BirthDate = x.BirthDate,
        }).ToList();

        var response = new GetAllUserQueryResult
        {
            Result = result,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
        };
        return await Ok(response);
    }
}

/// <summary>FluentValidation rules for <see cref="GetAllUserQuery"/>.</summary>
public class GetAllUserQueryValidator : AbstractValidator<GetAllUserQuery>
{
    /// <summary>Creates the validator.</summary>
    public GetAllUserQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize).InclusiveBetween(1, Pagination.MaxPageSize);
    }
}

/// <summary>Single user row in <see cref="GetAllUserQueryResult"/>.</summary>
public class UserQueryResultItem
{
    /// <summary>Identity user id.</summary>
    public string Id { get; set; }

    /// <summary>Login username.</summary>
    public string Username { get; set; }

    /// <summary>Email address.</summary>
    public string Email { get; set; }

    /// <summary>Personal number (decrypted for display).</summary>
    public string PNumber { get; set; }

    /// <summary>First name.</summary>
    public string FirstName { get; set; }

    /// <summary>Last name.</summary>
    public string LastName { get; set; }

    /// <summary>Phone number.</summary>
    public string Phone { get; set; }

    /// <summary>Department id.</summary>
    public int DepartmentId { get; set; }

    /// <summary>Date of birth.</summary>
    public DateTime BirthDate { get; set; }
}

/// <summary>Paginated user list response.</summary>
public class GetAllUserQueryResult : PagedResultBase
{
    /// <summary>Users for the current page.</summary>
    public List<UserQueryResultItem>? Result { get; set; }
}
