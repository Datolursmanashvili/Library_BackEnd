using Application.Shared;
using Domain.Entities.BookAuthorEntity;
using Shared;
using static Application.Commands.BookAuthorCommands.AddBookAuthorCommand;

namespace Application.Commands.BookAuthorCommands;

public class AddBookAuthorCommand : Command<BookAuthorCommandResult>
{
    public int ProductId { get; set; }
    public int AuthorId { get; set; }

    public override async Task<CommandExecutionResultGeneric<BookAuthorCommandResult>> ExecuteAsync()
    {
        if (ProductId <= 0 || AuthorId <= 0)
            return await Fail<BookAuthorCommandResult>("პროდუქტი და ავტორი სავალდებულოა");

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

    public class BookAuthorCommandResult
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int AuthorId { get; set; }
    }
}
