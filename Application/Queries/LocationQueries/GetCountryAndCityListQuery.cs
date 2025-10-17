using Application.Shared;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Application.Queries.LocationQueries;

public class GetCountryAndCityListQuery : Query<List<CountryWithCitiesResult>>
{
    public string? CountryName { get; set; }
    public string? CityName { get; set; }

    public override async Task<QueryExecutionResult<List<CountryWithCitiesResult>>> Execute()
    {
        var Countries = _appContext.Locations.Where(c => !c.IsDeleted && c.IsCountry == true && c.Parent == null).AsNoTracking();
        var Cities = _appContext.Locations.Where(c => !c.IsDeleted && c.IsCountry == false && c.Parent != null).AsNoTracking();

        if (!string.IsNullOrEmpty(CountryName))
        {
            Countries = Countries.Where(c => c.Name.Contains(CountryName));
        }

        if (!string.IsNullOrEmpty(CityName))
        {
            Cities = Cities.Where(c => c.Name.Contains(CityName));
        }

        var countriesWithCitiesQuery = Countries
            .GroupJoin(
                Cities,
                country => country.Id,
                city => city.ParentId,
                (country, cities) => new CountryWithCitiesResult
                {
                    Id = country.Id,
                    Name = country.Name,
                    Cities = cities.Select(city => new CityResult
                    {
                        Id = city.Id,
                        Name = city.Name
                    }).ToList(),
                });


        if (countriesWithCitiesQuery.Count() == 0)
            return await Fail("ქვეყნები ვერ მოიძებნა");


        var countriesWithCities = await countriesWithCitiesQuery.ToListAsync();

        return await Ok(countriesWithCities);
    }
}

public class CountryWithCitiesResult
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<CityResult>? Cities { get; set; }
}

public class CityResult
{
    public int Id { get; set; }
    public string Name { get; set; }
}
