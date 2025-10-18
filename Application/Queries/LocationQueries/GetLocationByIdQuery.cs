using Application.Commands.LocationCommands;
using Application.Shared;
using Shared;

namespace Application.Queries.LocationQueries;

public class GetLocationByIdQuery : Query<LocationResult?>
{
    public int Id { get; set; }

    public override async Task<QueryExecutionResult<LocationResult?>> Execute()
    {
        var country = _appContext.Locations.FirstOrDefault(l => l.Id == Id && l.IsCountry && !l.IsDeleted);
        if (country == null)
            return await Fail("ქვეყანა ვერ მოიძებნა.");

        var result = new LocationResult
        {
            Id = country.Id,
            Name = country.Name,
            IsCountry = country.IsCountry,
            ParentId = country.ParentId
        };

        return await Ok(result);
    }
}
