using Application.HelperMethods;
using Application.Shared;
using Domain.Entities.RoleEntity;
using FluentValidation;
using FluentValidation.Attributes;
using Infrastructure.Repositories;
using Shared;

namespace Application.Commands.RoleCommands
{
    [Validator(typeof(EditRoleCommandValidation))]
    public class EditRoleNameCommand : Command
    {
        public string? Id { get; set; }
        public string? RoleName { get; set; }
        public override async Task<RepositoryExecutionResult> ExecuteAsync()
        {
            if (applicationDbContext.Set<ApplicationRole>().Any(x => x.Name == RoleName && !x.IsDeleted && x.Id != Id))
                return await Fail("დეპარტამენტი_ამ_დასახელებით_უკვე_არსებობს");//დეპარტამენტი ამ დასახელებით უკვე არსებობს

            var role = applicationDbContext.Set<ApplicationRole>()
                                           .FirstOrDefault(x => x.Id == Id && !x.IsDeleted);

            if (role.IsNull())
            {
                return await Fail("ასეთი_როლი_ვერ_მოიძებნა");//ასეთი როლი ვერ მოიძებნა
            }
            if (RoleHelper.CheckRoleName(RoleName)) return await Fail($"როლის სახელი უნდა შეიცავდეს ციფრებს ან ლათინურ სიმბოლოებს");


            return await RoleRepository.EditRoleName(role, RoleName);
        }
    }
    public class EditRoleCommandValidation : AbstractValidator<EditRoleNameCommand>
    {
        public EditRoleCommandValidation()
        {
            RuleFor(x => x.RoleName).NotEmpty().WithMessage("სავალდებულო_ველია_როლის_სახელი");//"სავალდებულო ველია როლის სახელი"
            RuleFor(x => x.Id).NotEmpty().WithMessage("სავალდებულო_ველია_როლის_იდენტიფიკატორი");//"სავალდებულო ველია როლის იდენტიფიკატორი"
        }
    }
}
