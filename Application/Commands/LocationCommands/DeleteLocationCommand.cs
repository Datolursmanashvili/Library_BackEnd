using Application.Shared;
using Shared;

namespace Application.Commands.LocationCommands;

public class DeleteLocationCommand : Command<DeleteResult>
{
    public int Id { get; set; }

    public override async Task<CommandExecutionResultGeneric<DeleteResult>> ExecuteCommandLogicAsync()
    {
        if (Id <= 0)
            return await Fail<DeleteResult>("Id არასწორია");

        var location = await _locationRepository.GetByIdAsync(Id);
        if (location == null)
            return await Fail<DeleteResult>("მონაცემი ვერ მოიძებნა");

        if (location.Children?.Any() == true)
        {
            var childIds = string.Join(", ", location.Children.Select(x => x.Id));
            return await Fail<DeleteResult>($"მონაცემის წაშლა შეუძლებელია, რადგან მას აქვს შვილობილი ელემენტები id:  {childIds}");
        }

        var result = await _locationRepository.DeleteAsync(Id);
        if (!result.Success)
            return await Fail<DeleteResult>(result.ErrorMessage);

        return await Ok(new DeleteResult() { Success = true ,Message  = "წარმატებით წაიშალა" });
    }
}

public class DeleteResult
{
    public bool Success { get; set; }
    public string? Message { get; set; }
}