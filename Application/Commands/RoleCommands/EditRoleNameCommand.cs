using Application.HelperMethods;
using Application.Shared;
using Domain.Entities.RoleEntity;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Shared;

namespace Application.Commands.RoleCommands
{
    /// <summary>
    /// Rename an existing role (PUT JSON).
    /// </summary>
    public class EditRoleNameCommand : Command
    {
        /// <summary>Role id to update.</summary>
        public string? Id { get; set; }

        /// <summary>New role name.</summary>
        public string? RoleName { get; set; }

        /// <inheritdoc />
        public override async Task<RepositoryExecutionResult> ExecuteAsync()
        {
            if (applicationDbContext.Set<ApplicationRole>().Any(x => x.Name == RoleName && !x.IsDeleted && x.Id != Id))
                return await Fail(StatusCodes.Status409Conflict, "დეპარტამენტი_ამ_დასახელებით_უკვე_არსებობს");//დეპარტამენტი ამ დასახელებით უკვე არსებობს

            var role = applicationDbContext.Set<ApplicationRole>()
                                           .FirstOrDefault(x => x.Id == Id && !x.IsDeleted);

            if (role.IsNull())
            {
                return await Fail(StatusCodes.Status404NotFound, "ასეთი_როლი_ვერ_მოიძებნა");//ასეთი როლი ვერ მოიძებნა
            }
            if (RoleHelper.CheckRoleName(RoleName)) return await Fail(StatusCodes.Status400BadRequest, $"როლის სახელი უნდა შეიცავდეს ციფრებს ან ლათინურ სიმბოლოებს");


            return await RoleRepository.EditRoleName(role, RoleName);
        }
    }

    /// <summary>FluentValidation for <see cref="EditRoleNameCommand"/>.</summary>
    public class EditRoleCommandValidation : AbstractValidator<EditRoleNameCommand>
    {
        /// <summary>Creates the validator.</summary>
        public EditRoleCommandValidation()
        {
            RuleFor(x => x.RoleName).NotEmpty().WithMessage("სავალდებულო_ველია_როლის_სახელი");//"სავალდებულო ველია როლის სახელი"
            RuleFor(x => x.Id).NotEmpty().WithMessage("სავალდებულო_ველია_როლის_იდენტიფიკატორი");//"სავალდებულო ველია როლის იდენტიფიკატორი"
        }
    }
}
