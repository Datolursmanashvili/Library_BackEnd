using Application.Shared;
using Shared;

namespace Application.Commands.ProductsCommands;
public class DeleteProductCommand : Command<bool>
{
    public int Id { get; set; }

    public override async Task<CommandExecutionResultGeneric<bool>> ExecuteCommandLogicAsync()
    {
        if (Id <= 0)
            return await Fail<bool>("პროდუქტის Id არასწორია");

        var product = await _productRepository.GetByIdAsync(Id);
        if (product == null)
            return await Fail<bool>("პროდუქტი ვერ მოიძებნა");

        var result = await _productRepository.DeleteAsync(Id);
        if (!result.Success)
            return await Fail<bool>(result.ErrorMessage);

        return await Ok(true);
    }
}
