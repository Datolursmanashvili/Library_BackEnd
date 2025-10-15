using Domain.Entities.AuthorEntity;
using Domain.Entities.AuthorEntity.IRepository;
using Infrastructure.DB;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Infrastructure.Repositories;

public class AuthorRepository : BaseRepository, IAuthorRepository
{

    public AuthorRepository(ApplicationDbContext applicationDbContext, IServiceProvider serviceProvider)
        : base(applicationDbContext, serviceProvider)
    {
    }

    // CREATE
    public async Task<CommandExecutionResult> CreateAsync(Author model)
    {
        try
        {
            var resultId = await Insert<Author, int>(model);
            return new CommandExecutionResult { Success = true, ErrorMessage = "ავტორი შექმნილია წარმატებით.", ResultId = resultId.ToString() };
        }
        catch (Exception ex)
        {
            return new CommandExecutionResult { Success = false, ErrorMessage = ex.Message };
        }
    }

    // READ ALL
    public async Task<List<Author>?> GetAllAsync()
    {
        return await _ApplicationDbContext.Authors.ToListAsync();
    }

    // READ BY ID
    public async Task<Author?> GetByIdAsync(int id)
    {
        return await _ApplicationDbContext.Authors.FirstOrDefaultAsync(a => a.Id == id);
    }

    // UPDATE
    public async Task<CommandExecutionResult> UpdateAsync(Author model)
    {
        try
        {
            var existingAuthor = await _ApplicationDbContext.Authors.FindAsync(model.Id);
            if (existingAuthor == null)
                return new CommandExecutionResult { Success = false, ErrorMessage = "ავტორი ვერ მოიძებნა." };

            existingAuthor.FirstName = model.FirstName;
            existingAuthor.LastName = model.LastName;
            existingAuthor.Gender = model.Gender;
            existingAuthor.PersonalNumber = model.PersonalNumber;
            existingAuthor.BirthDate = model.BirthDate;
            existingAuthor.CountryId = model.CountryId;
            existingAuthor.CityId = model.CityId;
            existingAuthor.PhoneNumber = model.PhoneNumber;
            existingAuthor.Email = model.Email;

            _ApplicationDbContext.Authors.Update(existingAuthor);
            await _ApplicationDbContext.SaveChangesAsync();

            return new CommandExecutionResult { Success = true, ErrorMessage = "ავტორი განახლებულია წარმატებით." };
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
            var author = await _ApplicationDbContext.Authors.FindAsync(id);
            if (author == null)
                return new CommandExecutionResult { Success = false, ErrorMessage = "ავტორი ვერ მოიძებნა." };

            _ApplicationDbContext.Authors.Remove(author);
            await _ApplicationDbContext.SaveChangesAsync();

            return new CommandExecutionResult { Success = true, ErrorMessage = "ავტორი წაშლილია წარმატებით." };
        }
        catch (Exception ex)
        {
            return new CommandExecutionResult { Success = false, ErrorMessage = ex.Message };
        }
    }
}
