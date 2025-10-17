using Application.Shared;
using Shared;

namespace Application.Commands.LocationCommands;

public class DeleteLocationCommand : Command<bool>
{
    public int Id { get; set; }

    public override async Task<CommandExecutionResultGeneric<bool>> ExecuteCommandLogicAsync()
    {
        if (Id <= 0)
            return await Fail<bool>("Id არასწორია");

        var location = await _locationRepository.GetByIdAsync(Id);
        if (location == null)
            return await Fail<bool>("მონაცემი ვერ მოიძებნა");

        // Проверяем, есть ли дочерние записи (например, города у страны)
        if (location.Children?.Any() == true)
            return await Fail<bool>("მონაცემის წაშლა შეუძლებელია, რადგან მას აქვს შვილობილი ელემენტები");

        var result = await _locationRepository.DeleteAsync(Id);
        if (!result.Success)
            return await Fail<bool>(result.ErrorMessage);

        return await Ok(true);
    }
}
