using Application.Shared;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Application.Queries.AuthorQueries;

public class GetAllAuthorsByFiltresQuery : Query<GetAllAuthorsQueryResult>
{
    public int PageSize { get; set; } = 10;
    public int Page { get; set; } = 0;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Gender { get; set; }
    public int? CountryId { get; set; }
    public int? CityId { get; set; }
    public string? PersonalNumber { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public override async Task<QueryExecutionResult<GetAllAuthorsQueryResult>> Execute()
    {
        var query = _appContext.Authors.Where(a => !a.IsDeleted).AsNoTracking();

        if (!string.IsNullOrEmpty(FirstName))
            query = query.Where(a => a.FirstName.Contains(FirstName));

        if (!string.IsNullOrEmpty(LastName))
            query = query.Where(a => a.LastName.Contains(LastName));

        if (!string.IsNullOrEmpty(Gender))
            query = query.Where(a => a.Gender == Gender);

        if (CountryId.HasValue)
            query = query.Where(a => a.CountryId == CountryId.Value);

        if (CityId.HasValue)
            query = query.Where(a => a.CityId == CityId.Value);

        if (!string.IsNullOrEmpty(PersonalNumber))
            query = query.Where(a => a.PersonalNumber.Contains(PersonalNumber));

        if (!string.IsNullOrEmpty(PhoneNumber))
            query = query.Where(a => a.PhoneNumber.Contains(PhoneNumber));

        if (!string.IsNullOrEmpty(Email))
            query = query.Where(a => a.Email.Contains(Email));

        var totalCount = await query.CountAsync();

        var result = query
            .OrderBy(a => a.Id)
            .Skip(Page * PageSize)
            .Take(PageSize)
            .Select(a => new AuthorQueryResultItem
            {
                Id = a.Id,
                FirstName = a.FirstName,
                LastName = a.LastName,
                Gender = a.Gender,
                PersonalNumber = a.PersonalNumber,
                BirthDate = a.BirthDate,
                CountryId = a.CountryId,
                CityId = a.CityId,
                PhoneNumber = a.PhoneNumber,
                Email = a.Email
            })
            .ToList();

        var response = new GetAllAuthorsQueryResult
        {
            Result = result,
            TotalCount = totalCount
        };

        return await Ok(response);
    }
}

public class AuthorQueryResultItem
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Gender { get; set; }
    public string PersonalNumber { get; set; }
    public DateTime BirthDate { get; set; }
    public int CountryId { get; set; }
    public int CityId { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
}

public class GetAllAuthorsQueryResult
{
    public List<AuthorQueryResultItem>? Result { get; set; }
    public int TotalCount { get; set; }
}
