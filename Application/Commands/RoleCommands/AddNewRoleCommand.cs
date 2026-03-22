using Application.HelperMethods;
using Application.Shared;
using Domain.Entities.RoleEntity;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Shared;

namespace Application.Commands.RoleCommands
{
    /// <summary>
    /// Create a new role (POST JSON).
    /// </summary>
    public class AddNewRoleCommand : Command
    {
        /// <summary>New role name (Latin letters or digits per validation).</summary>
        public string RoleName { get; set; }

        /// <summary>Permissions to assign to the role.</summary>
        public IEnumerable<Permissions> Permissions { get; set; }

        /// <inheritdoc />
        public override async Task<RepositoryExecutionResult> ExecuteAsync()
        {
            if (RoleHelper.CheckRoleName(RoleName)) return await Fail(StatusCodes.Status400BadRequest, $"გთხოვთ დაარედაკტიროთ როლის სახელი როლის სახელი უნდა შეიცავდეს მხოლოდ ციფრებს ან ლათინურ სიმბოლოებს");

            return await RoleRepository.AddNewRole(new ApplicationRole() { Name = RoleName, Permissions = Permissions, NormalizedName = RoleName.ToUpper() });
        }
    }

    /// <summary>FluentValidation for <see cref="AddNewRoleCommand"/>.</summary>
    public class AddNewRoleCommandValidation : AbstractValidator<AddNewRoleCommand>
    {
        /// <summary>Creates the validator.</summary>
        public AddNewRoleCommandValidation()
        {
            RuleFor(x => x.RoleName).NotEmpty();
        }
    }
}
