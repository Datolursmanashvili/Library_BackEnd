using Application.Shared;
using FluentValidation;
using FluentValidation.Attributes;
using Shared;

namespace Application.Commands.PublisherCommands;

[Validator(typeof(UpdatePublisherCommandValidator))]
public class UpdatePublisherCommand : Command<PublisherCommandResult>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public override async Task<CommandExecutionResultGeneric<PublisherCommandResult>> ExecuteCommandLogicAsync()
    {
        var publisher = await _publisherRepository.GetByIdAsync(Id);
        if (publisher == null)
            return await Fail<PublisherCommandResult>("Publisher not found");

        publisher.Name = Name;

        var result = await _publisherRepository.UpdateAsync(publisher);
        if (!result.Success)
            return await Fail<PublisherCommandResult>(result.ErrorMessage);

        return await Ok(new PublisherCommandResult
        {
            Id = publisher.Id,
            Name = publisher.Name
        });
    }

    public class UpdatePublisherCommandValidator : AbstractValidator<UpdatePublisherCommand>
    {
        public UpdatePublisherCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Publisher Id is required");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MinimumLength(2).WithMessage("Name must be at least 2 characters")
                .MaximumLength(250).WithMessage("Name cannot exceed 250 characters");
        }
    }
}
