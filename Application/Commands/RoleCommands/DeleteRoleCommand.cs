using Application.Shared;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Shared;

namespace Application.Commands.RoleCommands
{
    public class DeleteRoleCommand : Command
    {
        public string Id { get; set; }

        public override async Task<RepositoryExecutionResult> ExecuteAsync()
        {
            var role = applicationDbContext.Roles.FirstOrDefault(x => x.Id == Id);

            if (applicationDbContext.UserRoles.Any(x => x.RoleId == Id))
            {
                return await Fail(StatusCodes.Status409Conflict, "როლის_წაშლა_ვერ_მოხერხდა_იუზერია_მიმაგრებული");//როლის წაშლა ვერ მოხერხდა,იუზერია მიმაგრებული
            }

            if (role.IsNull())
            {
                return await Fail(StatusCodes.Status404NotFound, "როლი_ვერ_მოიძებნა");//როლი ვერ მოიძებნა"
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
