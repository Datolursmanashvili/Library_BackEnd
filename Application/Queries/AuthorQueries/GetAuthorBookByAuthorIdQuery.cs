using Application.Shared;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Application.Queries.AuthorQueries;

public class GetAuthorBookByAuthorIdQuery : Query<AuthorQueryResultItem>
{
    public int Id { get; set; }

    public override async Task<QueryExecutionResult<AuthorQueryResultItem>> Execute()
    {
        var author = _appContext.Authors.FirstOrDefault(a => a.Id == Id && !a.IsDeleted);
        if (author == null)
            return await Fail("ავტორი ვერ მოიძებნა");


        var location = _appContext.Locations.Where(x => x.Id == author.CityId || x.Id == author.CountryId)
                                            .AsNoTracking()
                                            .ToDictionary(x => x.Id, x => x.Name);


        var result = new AuthorQueryResultItem
        {
            Id = author.Id,
            FirstName = author.FirstName,
            LastName = author.LastName,
            Gender = author.Gender,
            PersonalNumber = author.PersonalNumber,
            BirthDate = author.BirthDate,
            CountryId = author.CountryId,
            CityId = author.CityId,
            PhoneNumber = author.PhoneNumber,
            Email = author.Email,
            CityName = location.GetValueOrDefault(author.CityId),
            CountryName = location.GetValueOrDefault(author.CityId),
        };

        return await Ok(result);
    }
}
