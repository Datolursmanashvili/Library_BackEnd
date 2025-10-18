using Application.Shared;
using FluentValidation;
using FluentValidation.Attributes;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Application.Commands.AuthorCommands;


[Validator(typeof(UpdateAuthorCommandValidator))]
public class UpdateAuthorCommand : Command<AuthorCommandResult>
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Gender { get; set; }
    public string PersonalNumber { get; set; }
    public DateTime BirthDate { get; set; }
    public int CountryId { get; set; }
    public int CityId { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }

    public override async Task<CommandExecutionResultGeneric<AuthorCommandResult>> ExecuteCommandLogicAsync()
    {
        var locationResult = await applicationDbContext.Locations
                                                      .Where(x => x.Id == CountryId || x.Id == CityId)
                                                      .ToListAsync();

        if (!locationResult.Any(x => x.Id == CountryId))
            return await Fail<AuthorCommandResult>("ასეთი ქვეყანა არ არსებობს");

        if (!locationResult.Any(x => x.Id == CityId))
            return await Fail<AuthorCommandResult>("ასეთი ქალაქი არ არსებობს");

        var author = await _authorRepository.GetByIdAsync(Id);
        if (author == null)
            return await Fail<AuthorCommandResult>("ავტორი ვერ მოიძებნა");

        author.FirstName = FirstName;
        author.LastName = LastName;
        author.Gender = Gender;
        author.PersonalNumber = PersonalNumber;
        author.BirthDate = BirthDate;
        author.CountryId = CountryId;
        author.CityId = CityId;
        author.PhoneNumber = PhoneNumber;
        author.Email = Email;

        var result = await _authorRepository.UpdateAsync(author);
        if (!result.Success)
            return await Fail<AuthorCommandResult>(result.ErrorMessage);

        return await Ok(new AuthorCommandResult
        {
            Id = author.Id,
            FirstName = author.FirstName,
            LastName = author.LastName,
            Gender = author.Gender,
            PersonalNumber = author.PersonalNumber,
            BirthDate = author.BirthDate,
            CountryId = author.CountryId,
            CityId = author.CityId,
            PhoneNumber = author.PhoneNumber,
            Email = author.Email
        });
    }

    public class UpdateAuthorCommandValidator : AbstractValidator<UpdateAuthorCommand>
    {
        public UpdateAuthorCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("ავტორის Id სავალდებულოა");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("სახელი სავალდებულოა")
                .MinimumLength(2).WithMessage("სახელი უნდა იყოს მინიმუმ 2 სიმბოლო")
                .MaximumLength(50).WithMessage("სახელი მაქსიმუმ 50 სიმბოლო შეიძლება იყოს");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("გვარი სავალდებულოა")
                .MinimumLength(2).WithMessage("გვარი უნდა იყოს მინიმუმ 2 სიმბოლო")
                .MaximumLength(50).WithMessage("გვარი მაქსიმუმ 50 სიმბოლო შეიძლება იყოს");

            RuleFor(x => x.Gender)
                .NotEmpty().WithMessage("სქესი სავალდებულოა")
                .Must(g => g == "კაცი" || g == "ქალი")
                .WithMessage("სქესი უნდა იყოს 'კაცი' ან 'ქალი'");

            RuleFor(x => x.PersonalNumber)
                .NotEmpty().WithMessage("პირადი ნომერი სავალდებულოა")
                .Matches(@"^\d{11}$").WithMessage("პირადი ნომერი უნდა შეიცავდეს 11 ციფრს");

            RuleFor(x => x.BirthDate)
                .NotEmpty().WithMessage("დაბადების თარიღი სავალდებულოა")
                .Must(bd =>
                {
                    var age = DateTime.Today.Year - bd.Year;
                    if (bd.Date > DateTime.Today.AddYears(-age)) age--;
                    return age >= 18;
                }).WithMessage("ავტორი უნდა იყოს მინიმუმ 18 წლის");

            RuleFor(x => x.CountryId)
                .GreaterThan(0).WithMessage("ქვეყანა სავალდებულოა");

            RuleFor(x => x.CityId)
                .GreaterThan(0).WithMessage("ქალაქი სავალდებულოა");

            RuleFor(x => x.PhoneNumber)
                .MinimumLength(4).WithMessage("ტელეფონის ნომერი უნდა იყოს მინიმუმ 4 სიმბოლო")
                .MaximumLength(50).WithMessage("ტელეფონის ნომერი მაქსიმუმ 50 სიმბოლო შეიძლება იყოს");

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("მეილის ფორმატი არასწორია");
        }
    }
}
