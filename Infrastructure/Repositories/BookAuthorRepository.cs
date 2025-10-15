using Domain.Entities.BookAuthorEntity;
using Domain.Entities.BookAuthorEntity.IRepository;
using Infrastructure.DB;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Infrastructure.Repositories;

public class BookAuthorRepository : BaseRepository, IBookAuthorRepository
{
    public BookAuthorRepository(ApplicationDbContext applicationDbContext, IServiceProvider serviceProvider)
        : base(applicationDbContext, serviceProvider)
    {
    }

    // CREATE
    public async Task<CommandExecutionResult> CreateAsync(BookAuthor model)
    {
        try
        {
            var resultId = await Insert<BookAuthor, int>(model);
            return new CommandExecutionResult
            {
                Success = true,
                ErrorMessage = "ჩანაწერი შექმნილია წარმატებით.",
                ResultId = resultId.ToString()
            };
        }
        catch (Exception ex)
        {
            return new CommandExecutionResult { Success = false, ErrorMessage = ex.Message };
        }
    }

    // READ ALL
    public async Task<List<BookAuthor>?> GetAllAsync()
    {
        return await _ApplicationDbContext.BookAuthors
            .Include(x => x.Product)
            .Include(x => x.Author)
            .ToListAsync();
    }

    // READ BY ID
    public async Task<BookAuthor?> GetByIdAsync(int id)
    {
        return await _ApplicationDbContext.BookAuthors
            .Include(x => x.Product)
            .Include(x => x.Author)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    // UPDATE
    public async Task<CommandExecutionResult> UpdateAsync(BookAuthor model)
    {
        try
        {
            var existing = await _ApplicationDbContext.BookAuthors.FindAsync(model.Id);
            if (existing == null)
                return new CommandExecutionResult { Success = false, ErrorMessage = "ჩანაწერი ვერ მოიძებნა." };

            existing.ProductId = model.ProductId;
            existing.AuthorId = model.AuthorId;

            _ApplicationDbContext.BookAuthors.Update(existing);
            await _ApplicationDbContext.SaveChangesAsync();

            return new CommandExecutionResult { Success = true, ErrorMessage = "ჩანაწერი განახლებულია წარმატებით." };
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
            var entity = await _ApplicationDbContext.BookAuthors.FindAsync(id);
            if (entity == null)
                return new CommandExecutionResult { Success = false, ErrorMessage = "ჩანაწერი ვერ მოიძებნა." };

            _ApplicationDbContext.BookAuthors.Remove(entity);
            await _ApplicationDbContext.SaveChangesAsync();

            return new CommandExecutionResult { Success = true, ErrorMessage = "ჩანაწერი წაშლილია წარმატებით." };
        }
        catch (Exception ex)
        {
            return new CommandExecutionResult { Success = false, ErrorMessage = ex.Message };
        }
    }

}
