using Application.Shared;
using Domain.Entities.BookAuthorEntity;
using Shared;

namespace Application.Commands.BookAuthorCommands;

public class DeleteBookAuthorCommand : Command<bool>
{
    public int Id { get; set; }

    public override async Task<CommandExecutionResultGeneric<bool>> ExecuteAsync()
    {
        if (Id <= 0)
            return await Fail<bool>("არასწორი ID.");

        var result = await _bookAuthorRepository.DeleteAsync(Id);

        if (!result.Success)
            return await Fail<bool>(result.ErrorMessage);

        return await Ok(true);
    }
}
