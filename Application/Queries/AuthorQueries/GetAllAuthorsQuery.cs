using Application.Shared;
using Shared;

namespace Application.Queries.AuthorQueries;

public class GetAllAuthorsQuery : Query<GetAllAuthorsQueryResult>
{
    public int PageSize { get; set; } = 10;
    public int Page { get; set; } = 0;

    public override async Task<QueryExecutionResult<GetAllAuthorsQueryResult>> Execute()
    {
        var authors = _appContext.Authors.AsQueryable().Where(a => !a.IsDeleted);
        var totalCount = authors.Count();

        var result = authors
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
