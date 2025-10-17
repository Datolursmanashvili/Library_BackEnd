using Application.Commands.LocationCommands;
using Application.Shared;
using Shared;

namespace Application.Queries.LocationQueries;

public class GetAllCountryQuery : Query<List<LocationResult>?>
{
    public override async Task<QueryExecutionResult<List<LocationResult>?>> Execute()
    {
        var countries = _appContext.Locations
            .Where(l => l.IsCountry && !l.IsDeleted)
            .Select(l => new LocationResult
            {
                Id = l.Id,
                Name = l.Name,
                IsCountry = l.IsCountry,
                ParentId = l.ParentId
            })
            .ToList();

        return await Ok(countries);
    }
}
