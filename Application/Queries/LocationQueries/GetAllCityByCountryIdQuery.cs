using Application.Commands.LocationCommands;
using Application.Shared;
using Shared;

namespace Application.Queries.LocationQueries;

public class GetAllCityByCountryIdQuery : Query<List<LocationResult>?>
{
    public int CountryId { get; set; }

    public override async Task<QueryExecutionResult<List<LocationResult>?>> Execute()
    {
        var cities = _appContext.Locations
            .Where(l => !l.IsCountry && l.ParentId == CountryId && !l.IsDeleted)
            .Select(l => new LocationResult
            {
                Id = l.Id,
                Name = l.Name,
                IsCountry = l.IsCountry,
                ParentId = l.ParentId
            })
            .ToList();

        return await Ok(cities);
    }
}
