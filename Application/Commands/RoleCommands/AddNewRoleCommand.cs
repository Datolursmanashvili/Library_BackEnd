using Application.HelperMethods;
using Application.Shared;
using Domain.Entities.RoleEntity;
using FluentValidation;
using FluentValidation.Attributes;
using Shared;

namespace Application.Commands.RoleCommands
{
    [Validator(typeof(AddNewRoleCommandValidation))]
    public class AddNewRoleCommand : Command
    {
        public string RoleName { get; set; }
        public IEnumerable<Permissions> Permissions { get; set; }
        public override async Task<RepositoryExecutionResult> ExecuteAsync()
        {
            if (RoleHelper.CheckRoleName(RoleName)) return await Fail($"გთხოვთ დაარედაკტიროთ როლის სახელი როლის სახელი უნდა შეიცავდეს მხოლოდ ციფრებს ან ლათინურ სიმბოლოებს");

            return await RoleRepository.AddNewRole(new ApplicationRole() { Name = RoleName, Permissions = Permissions, NormalizedName = RoleName.ToUpper() });
        }
    }
    public class AddNewRoleCommandValidation : AbstractValidator<AddNewRoleCommand>
    {
        public AddNewRoleCommandValidation()
        {
            RuleFor(x => x.RoleName).NotEmpty();
            //RuleFor(x => x.Permissions).Must(x => x.Count() > 0);
        }
    }
}
