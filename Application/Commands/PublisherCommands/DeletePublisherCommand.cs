using Application.Commands.LocationCommands;
using Application.Shared;
using Shared;

namespace Application.Commands.PublisherCommands;

public class DeletePublisherCommand : Command<DeleteResult>
{
    public int Id { get; set; }

    public override async Task<CommandExecutionResultGeneric<DeleteResult>> ExecuteCommandLogicAsync()
    {
        if (Id <= 0)
            return await Fail<DeleteResult>("Publisher Id is invalid");

        var result = await _publisherRepository.DeleteAsync(Id);
        if (!result.Success)
            return await Fail<DeleteResult>(result.ErrorMessage);

        return await Ok(new DeleteResult() { Success= true , Message = "წარმატებით წაიშალა"});
    }
}
