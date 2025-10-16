using Application.Shared;
using Domain.Entities.BookAuthorEntity;
using Shared;

namespace Application.Commands.BookAuthorCommands;

public class AddBookAuthorCommand : Command<BookAuthorCommandResult>
{
    public int ProductId { get; set; }
    public int AuthorId { get; set; }

    public override async Task<CommandExecutionResultGeneric<BookAuthorCommandResult>> ExecuteCommandLogicAsync()
    {
        if (ProductId <= 0 || AuthorId <= 0)
            return await Fail<BookAuthorCommandResult>("პროდუქტი და ავტორი სავალდებულოა");

        if (applicationDbContext.Authors.SingleOrDefault(x => x.Id == AuthorId && x.IsDeleted == false).IsNull())
            return await Fail<BookAuthorCommandResult>("ასეთი ავტორი ვერ მოიძებნა");

        if (applicationDbContext.Products.SingleOrDefault(x => x.Id == ProductId && x.IsDeleted == false).IsNull())
            return await Fail<BookAuthorCommandResult>("ასეთი პროდუქტი ვერ მოიძებნა");

        if (applicationDbContext.BookAuthors.SingleOrDefault(x => x.ProductId == ProductId && x.AuthorId == AuthorId && x.IsDeleted == false).IsNotNull())
            return await Fail<BookAuthorCommandResult>("ასეთი წიგნი და ავტორი უკვე არსებობს სისტემაში");

        var model = new BookAuthor
        {
            ProductId = ProductId,
            AuthorId = AuthorId,
            CreatedAt = DateTime.Now,
            IsDeleted = false
        };

        var result = await _bookAuthorRepository.CreateAsync(model);
        if (!result.Success)
            return await Fail<BookAuthorCommandResult>(result.ErrorMessage);

        return await Ok(new BookAuthorCommandResult
        {
            Id = model.Id,
            ProductId = model.ProductId,
            AuthorId = model.AuthorId
        });
    }
}
public class BookAuthorCommandResult
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int AuthorId { get; set; }
}