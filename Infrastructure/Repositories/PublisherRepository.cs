using Domain.Entities.PublisherEntity;
using Domain.Entities.PublisherEntity.IRepository;
using Infrastructure.DB;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Infrastructure.Repositories
{
    public class PublisherRepository : BaseRepository, IPublisherRepository
    {
        public PublisherRepository(ApplicationDbContext applicationDbContext, IServiceProvider serviceProvider)
            : base(applicationDbContext, serviceProvider)
        {
        }

        // CREATE
        public async Task<CommandExecutionResult> CreateAsync(Publisher model)
        {
            try
            {
                var resultId = await Insert<Publisher, int>(model);
                return new CommandExecutionResult { Success = true, ResultId = resultId.ToString(), ErrorMessage = "Publisher created successfully." };
            }
            catch (Exception ex)
            {
                return new CommandExecutionResult { Success = false, ErrorMessage = ex.Message };
            }
        }

        // READ ALL
        public async Task<List<Publisher>?> GetAllAsync()
        {
            return await _ApplicationDbContext.Publishers.Where(p => !p.IsDeleted).ToListAsync();
        }

        // READ BY ID
        public async Task<Publisher?> GetByIdAsync(int id)
        {
            return await _ApplicationDbContext.Publishers.FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
        }

        // UPDATE
        public async Task<CommandExecutionResult> UpdateAsync(Publisher model)
        {
            try
            {
                var existing = await _ApplicationDbContext.Publishers.FindAsync(model.Id);
                if (existing == null)
                    return new CommandExecutionResult { Success = false, ErrorMessage = "Publisher not found." };

                existing.Name = model.Name;

                _ApplicationDbContext.Publishers.Update(existing);
                await _ApplicationDbContext.SaveChangesAsync();

                return new CommandExecutionResult { Success = true, ErrorMessage = "Publisher updated successfully." };
            }
            catch (Exception ex)
            {
                return new CommandExecutionResult { Success = false, ErrorMessage = ex.Message };
            }
        }

        // DELETE
        public async Task<CommandExecutionResult> DeleteAsync(int id)
        {
            try
            {
                var publisher = await _ApplicationDbContext.Publishers.FindAsync(id);
                if (publisher == null)
                    return new CommandExecutionResult { Success = false, ErrorMessage = "Publisher not found." };

                _ApplicationDbContext.Publishers.Remove(publisher);
                await _ApplicationDbContext.SaveChangesAsync();

                return new CommandExecutionResult { Success = true, ErrorMessage = "Publisher deleted successfully." };
            }
            catch (Exception ex)
            {
                return new CommandExecutionResult { Success = false, ErrorMessage = ex.Message };
            }
        }
    }
}
