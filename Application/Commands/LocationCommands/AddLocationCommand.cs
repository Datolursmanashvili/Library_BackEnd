using Application.Shared;
using Domain.Entities.LocationEntity;
using FluentValidation;
using FluentValidation.Attributes;
using Shared;

namespace Application.Commands.LocationCommands;

[Validator(typeof(AddLocationCommandValidator))]
public class AddLocationCommand : Command<LocationResult>
{
    public string Name { get; set; } = string.Empty;
    public bool IsCountry { get; set; }
    public int? ParentId { get; set; }

    public override async Task<CommandExecutionResultGeneric<LocationResult>> ExecuteCommandLogicAsync()
    {
        var location = new Location
        {
            Name = Name,
            IsCountry = IsCountry,
            ParentId = IsCountry ? null : ParentId,
            CreatedAt = DateTime.Now,
            IsDeleted = false
        };

        if (IsCountry && ParentId != null) return await Fail<LocationResult>("ქვეყანას არ უნდა ყავდეს მშობელი");

        var result = await _locationRepository.CreateAsync(location);

        if (!result.Success)
            return await Fail<LocationResult>(result.ErrorMessage);

        return await Ok(new LocationResult
        {
            Id = Convert.ToInt32(result.ResultId),
            Name = location.Name,
            IsCountry = location.IsCountry,
            ParentId = location.ParentId
        });
    }

    public class AddLocationCommandValidator : AbstractValidator<AddLocationCommand>
    {
        public AddLocationCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("დასახელება სავალდებულოა")
                .MinimumLength(2).WithMessage("დასახელება უნდა იყოს მინიმუმ 2 სიმბოლო")
                .MaximumLength(200).WithMessage("დასახელება მაქსიმუმ 200 სიმბოლო შეიძლება იყოს");

            RuleFor(x => x.IsCountry)
                .NotNull().WithMessage("ველი სავალდებულოა");

            RuleFor(x => x.ParentId)
                .Must((cmd, parentId) =>
                {
                    // Если это страна — ParentId должно быть null
                    return cmd.IsCountry ? parentId == null : parentId > 0;
                })
                .WithMessage("ქალაქისთვის აუცილებელია ქვეყნის მითითება, ხოლო ქვეყნისთვის ParentId უნდა იყოს ცარიელი.");
        }
    }
}

public class LocationResult
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsCountry { get; set; }
    public int? ParentId { get; set; }
}