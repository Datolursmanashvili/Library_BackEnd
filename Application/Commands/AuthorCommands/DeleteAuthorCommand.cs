using Application.Shared;
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

        var result = await _authorRepository.DeleteAsync(Id);
        if (!result.Success)
            return await Fail<bool>(result.ErrorMessage);

        return await Ok(true);
    }
}
