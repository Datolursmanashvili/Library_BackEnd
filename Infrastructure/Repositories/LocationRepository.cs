using Domain.Entities.LocationEntity;
using Domain.Entities.LocationEntity.IRepository;
using Infrastructure.DB;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Infrastructure.Repositories
{
    public class LocationRepository : BaseRepository, ILocationRepository
    {
        public LocationRepository(ApplicationDbContext applicationDbContext, IServiceProvider serviceProvider)
            : base(applicationDbContext, serviceProvider)
        {
        }

        // CREATE
        public async Task<CommandExecutionResult> CreateAsync(Location model)
        {
            try
            {
                var resultId = await Insert<Location, int>(model);
                return new CommandExecutionResult
                {
                    Success = true,
                    ErrorMessage = "მონაცემი წარმატებით შეიქმნა.",
                    ResultId = resultId.ToString()
                };
            }
            catch (Exception ex)
            {
                return new CommandExecutionResult
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        // READ ALL
        public async Task<List<Location>> GetAllAsync()
        {
            return await _ApplicationDbContext.Locations
                .Include(l => l.Children)
                .ToListAsync();
        }

        // READ COUNTRIES WITH CITIES
        public async Task<List<Location>> GetCountriesWithCitiesAsync()
        {
            return await _ApplicationDbContext.Locations
                .Where(l => l.IsCountry)
                .Include(l => l.Children)
                .ToListAsync();
        }

        // READ BY ID
        public async Task<Location?> GetByIdAsync(int id)
        {
            return await _ApplicationDbContext.Locations
                .Include(l => l.Children)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        // UPDATE
        public async Task<CommandExecutionResult> UpdateAsync(Location model)
        {
            try
            {
                var existing = await _ApplicationDbContext.Locations.FindAsync(model.Id);
                if (existing == null)
                    return new CommandExecutionResult
                    {
                        Success = false,
                        ErrorMessage = "მონაცემი ვერ მოიძებნა."
                    };

                existing.Name = model.Name;
                existing.IsCountry = model.IsCountry;
                existing.ParentId = model.ParentId;

                _ApplicationDbContext.Locations.Update(existing);
                await _ApplicationDbContext.SaveChangesAsync();

                return new CommandExecutionResult
                {
                    Success = true,
                    ErrorMessage = "მონაცემი წარმატებით განახლდა."
                };
            }
            catch (Exception ex)
            {
                return new CommandExecutionResult
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        // DELETE
        public async Task<CommandExecutionResult> DeleteAsync(int id)
        {
            try
            {
                var location = await _ApplicationDbContext.Locations.FindAsync(id);
                if (location == null)
                    return new CommandExecutionResult
                    {
                        Success = false,
                        ErrorMessage = "მონაცემი ვერ მოიძებნა."
                    };

                _ApplicationDbContext.Locations.Remove(location);
                await _ApplicationDbContext.SaveChangesAsync();

                return new CommandExecutionResult
                {
                    Success = true,
                    ErrorMessage = "მონაცემი წარმატებით წაიშალა."
                };
            }
            catch (Exception ex)
            {
                return new CommandExecutionResult
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }
    }
}
