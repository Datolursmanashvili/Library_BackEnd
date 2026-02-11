using Domain.Entities.RoleEntity;
using Shared;

namespace Domain.Entities.RoleEntity.IRepository;

public interface IRoleRepository
{
    Task<RepositoryExecutionResult> AddNewRole(ApplicationRole role);
    Task<RepositoryExecutionResult> EditRoleName(ApplicationRole role, string roleName);
    Task<RepositoryExecutionResult> EditRolePermissions(ApplicationRole role, IEnumerable<Permissions>? permissions);
    Task<RepositoryExecutionResult> DeleteRole(ApplicationRole role);
}
