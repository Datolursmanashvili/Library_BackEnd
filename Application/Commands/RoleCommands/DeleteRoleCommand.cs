using Application.Shared;
using FluentValidation;
using FluentValidation.Attributes;
using Shared;

namespace Application.Commands.RoleCommands
{
    [Validator(typeof(DeleteRoleCommandValidation))]
    public class DeleteRoleCommand : Command
    {
        public string Id { get; set; }

        public override async Task<RepositoryExecutionResult> ExecuteAsync()
        {
            var role = applicationDbContext.Roles.FirstOrDefault(x => x.Id == Id);

            if (applicationDbContext.UserRoles.Any(x => x.RoleId == Id))
            {
                return await Fail("როლის_წაშლა_ვერ_მოხერხდა_იუზერია_მიმაგრებული");//როლის წაშლა ვერ მოხერხდა,იუზერია მიმაგრებული
            }

            if (role.IsNull())
            {
                return await Fail("როლი_ვერ_მოიძებნა");//როლი ვერ მოიძებნა"
            }

            return await RoleRepository.DeleteRole(role);
        }
    }
    public class DeleteRoleCommandValidation : AbstractValidator<DeleteRoleCommand>
    {
        public DeleteRoleCommandValidation()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
