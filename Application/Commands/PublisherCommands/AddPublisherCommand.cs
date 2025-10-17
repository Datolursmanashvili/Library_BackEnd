using Application.Shared;
using Domain.Entities.PublisherEntity;
using FluentValidation;
using FluentValidation.Attributes;
using Shared;

namespace Application.Commands.PublisherCommands;

[Validator(typeof(AddPublisherCommandValidator))]
public class AddPublisherCommand : Command<PublisherCommandResult>
{
    public string Name { get; set; } = string.Empty;

    public override async Task<CommandExecutionResultGeneric<PublisherCommandResult>> ExecuteCommandLogicAsync()
    {
        var publisher = new Publisher
        {
            Name = Name,
            CreatedAt = DateTime.Now,
            IsDeleted = false
        };

        var result = await _publisherRepository.CreateAsync(publisher);

        if (!result.Success)
            return await Fail<PublisherCommandResult>(result.ErrorMessage);

        return await Ok(new PublisherCommandResult
        {
            Id = Convert.ToInt32(result.ResultId),
            Name = publisher.Name
        });
    }

    public class AddPublisherCommandValidator : AbstractValidator<AddPublisherCommand>
    {
        public AddPublisherCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MinimumLength(2).WithMessage("Name must be at least 2 characters")
                .MaximumLength(250).WithMessage("Name cannot exceed 250 characters");
        }
    }
}

public class PublisherCommandResult
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

