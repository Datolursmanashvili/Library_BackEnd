using Domain.Entities.RoleEntity;
using Shared;

namespace Domain.Entities.RoleEntity.IRepository;

public interface IRoleRepository
{
    Task<CommandExecutionResult> AddNewRole(ApplicationRole role);
    Task<CommandExecutionResult> EditRoleName(ApplicationRole role, string roleName);
    Task<CommandExecutionResult> EditRolePermissions(ApplicationRole role, IEnumerable<Permissions>? permissions);
    Task<CommandExecutionResult> DeleteRole(ApplicationRole role);
}
