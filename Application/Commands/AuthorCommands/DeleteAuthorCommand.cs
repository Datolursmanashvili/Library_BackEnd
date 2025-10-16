using Application.Shared;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Application.Commands.AuthorCommands;

public class DeleteAuthorCommand : Command<bool>
{
    public int Id { get; set; }

    public override async Task<CommandExecutionResultGeneric<bool>> ExecuteCommandLogicAsync()
    {
        if (Id <= 0)
            return await Fail<bool>("ავტორის Id არასწორია");

        var author = await _authorRepository.GetByIdAsync(Id);
        if (author == null)
            return await Fail<bool>("ავტორი ვერ მოიძებნა");

        var booksForAuthor = await applicationDbContext.BookAuthors.Where(x => x.AuthorId == Id && x.IsDeleted == false).Select(x => x.ProductId).ToListAsync();

        if (booksForAuthor.IsNotNull())
            return await Fail<bool>($"ავტორის წაშლა შეუძლებელია გთხოთ წაშალოთ ავტორზე მიბმული წიგნები  {string.Join(", ", booksForAuthor)} ");

        var result = await _authorRepository.DeleteAsync(Id);
        if (!result.Success)
            return await Fail<bool>(result.ErrorMessage);

        return await Ok(true);
    }
}
