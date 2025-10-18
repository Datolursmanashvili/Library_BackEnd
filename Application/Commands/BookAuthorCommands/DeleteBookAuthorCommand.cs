using Application.Commands.LocationCommands;
using Application.Shared;
using Shared;

namespace Application.Commands.BookAuthorCommands;

public class DeleteBookAuthorCommand : Command<DeleteResult>
{
    public int Id { get; set; }

    public override async Task<CommandExecutionResultGeneric<DeleteResult>> ExecuteCommandLogicAsync()
    {
        if (Id <= 0)
            return await Fail<DeleteResult>("არასწორი ID.");

        var result = await _bookAuthorRepository.DeleteAsync(Id);

        if (!result.Success)
            return await Fail<DeleteResult>(result.ErrorMessage);

        return await Ok(new DeleteResult() { Success = true, Message = "წარმატებით წაიშალა"});
    }
}
