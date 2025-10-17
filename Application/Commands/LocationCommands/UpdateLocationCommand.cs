using Application.Shared;
using Domain.Entities.LocationEntity;
using FluentValidation;
using FluentValidation.Attributes;
using Shared;

namespace Application.Commands.LocationCommands;

[Validator(typeof(UpdateLocationCommandValidator))]
public class UpdateLocationCommand : Command<LocationResult>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsCountry { get; set; }
    public int? ParentId { get; set; }

    public override async Task<CommandExecutionResultGeneric<LocationResult>> ExecuteCommandLogicAsync()
    {
        var location = await _locationRepository.GetByIdAsync(Id);
        if (location == null)
            return await Fail<LocationResult>("მონაცემი ვერ მოიძებნა");

        location.Name = Name;
        location.IsCountry = IsCountry;
        location.ParentId = ParentId;

        var result = await _locationRepository.UpdateAsync(location);
        if (!result.Success)
            return await Fail<LocationResult>(result.ErrorMessage);


        if (IsCountry && ParentId != null) return await Fail<LocationResult>("ქვეყანას არ უნდა ყავდეს მშობელი");


        return await Ok(new LocationResult
        {
            Id = location.Id,
            Name = location.Name,
            IsCountry = location.IsCountry,
            ParentId = location.ParentId
        });
    }

    public class UpdateLocationCommandValidator : AbstractValidator<UpdateLocationCommand>
    {
        public UpdateLocationCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id სავალდებულოა");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("დასახელება სავალდებულოა")
                .MinimumLength(2).WithMessage("დასახელება უნდა იყოს მინიმუმ 2 სიმბოლო")
                .MaximumLength(200).WithMessage("დასახელება მაქსიმუმ 200 სიმბოლო შეიძლება იყოს");

            RuleFor(x => x.ParentId)
                .Must((cmd, parentId) =>
                {
                    return cmd.IsCountry ? parentId == null : parentId > 0;
                })
                .WithMessage("ქალაქისთვის აუცილებელია ქვეყნის მითითება, ხოლო ქვეყნისთვის ParentId უნდა იყოს ცარიელი.");
        }
    }
}
