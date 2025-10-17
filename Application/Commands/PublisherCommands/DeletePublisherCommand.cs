using Application.Shared;
using Shared;

namespace Application.Commands.PublisherCommands;

public class DeletePublisherCommand : Command<bool>
{
    public int Id { get; set; }

    public override async Task<CommandExecutionResultGeneric<bool>> ExecuteCommandLogicAsync()
    {
        if (Id <= 0)
            return await Fail<bool>("Publisher Id is invalid");

        var result = await _publisherRepository.DeleteAsync(Id);
        if (!result.Success)
            return await Fail<bool>(result.ErrorMessage);

        return await Ok(true);
    }
}
