using Domain.Entities.RoleEntity;
using Domain.Entities.RoleEntity.IRepository;
using Infrastructure.DB;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Infrastructure.Repositories;

public class RoleRepository : BaseRepository<ApplicationDbContext>, IRoleRepository
{
    public RoleManager<ApplicationRole> _roleManager { get; set; }
    public RoleRepository(ApplicationDbContext applicationDbContext,
        IServiceProvider serviceProvider,
        RoleManager<ApplicationRole> roleManager) : base(applicationDbContext, serviceProvider)
    {
        _roleManager = roleManager;
    }

    public async Task<RepositoryExecutionResult> AddNewRole(ApplicationRole role)
    {
        try
        {
            if (_ApplicationDbContext.Roles.Any(x => x.Name == role.Name && !x.IsDeleted))
            {
                return new RepositoryExecutionResult() { Success = false, Code = 409, ErrorMessage = "როლის_დასახელება_უკვე_გამოყენებულია " };//"როლის დასახელება უკვე გამოყენებულია"
            }

            if (_ApplicationDbContext.Roles.Any(x => x.Name == role.Name && x.IsDeleted))
            {
                var oldrole = _ApplicationDbContext.Roles.FirstOrDefault(x => x.Name == role.Name && x.IsDeleted);
                await _roleManager.DeleteAsync(oldrole);
            }

            await _roleManager.CreateAsync(role);
            await _roleManager.UpdateAsync(role);




            return new RepositoryExecutionResult() { Success = true };
        }
        catch (Exception ex)
        {

            return new RepositoryExecutionResult()
            {
                Success = false,
                Code = 500,
                ErrorMessage = ex.Message
            };
        }
    }

    public async Task<RepositoryExecutionResult> EditRoleName(ApplicationRole role, string roleName)
    {
        try
        {
            var oldRecord = new ApplicationRole()
            {
                Id = role.Id,
                Name = role.Name,
                NormalizedName = role.NormalizedName,
                Permissions = role.Permissions,
                UpdatedAt = role.UpdatedAt,
                ConcurrencyStamp = role.ConcurrencyStamp,
                CreatedAt = role.CreatedAt,
                DeletedAt = role.DeletedAt,
                IsDeleted = role.IsDeleted,
            };


            role.Name = roleName;
            role.UpdatedAt = DateTime.Now;
            await _roleManager.UpdateAsync(role);




            return new RepositoryExecutionResult() { Success = true };
        }
        catch (Exception ex)
        {
            return new RepositoryExecutionResult()
            {
                Success = false,
                Code = 500,
                ErrorMessage = ex.Message
            };
        }
    }
    public async Task<RepositoryExecutionResult> DeleteRole(ApplicationRole role)
    {
        try
        {
            role.IsDeleted = true;
            role.DeletedAt = DateTime.Now;

            await _roleManager.UpdateAsync(role);


            return new RepositoryExecutionResult() { Success = true };
        }
        catch (Exception ex)
        {
            return new RepositoryExecutionResult()
            {
                Success = false,
                Code = 500,
                ErrorMessage = ex.Message
            };
        }
    }

    public async Task<RepositoryExecutionResult> EditRolePermissions(ApplicationRole role, IEnumerable<Permissions>? permissions)
    {
        try
        {
            role.Permissions = permissions;
            role.UpdatedAt = DateTime.Now;
            await _roleManager.UpdateAsync(role);



            return new RepositoryExecutionResult() { Success = true };
        }
        catch (Exception ex)
        {
            return new RepositoryExecutionResult()
            {
                Success = false,
                Code = 500,
                ErrorMessage = ex.Message
            };
        }
    }

    public async Task<(string? RoleName, RepositoryExecutionResult Result)> GetRoleNameByUserIdAsync(string userId)
    {
        var result = new RepositoryExecutionResult();

        try
        {
            var roleName = await (
                from user in _ApplicationDbContext.Users
                join userRole in _ApplicationDbContext.UserRoles on user.Id equals userRole.UserId
                join role in _ApplicationDbContext.Roles on userRole.RoleId equals role.Id
                where user.Id == userId 
                select role.Name
            ).FirstOrDefaultAsync();

            if (roleName == null)
            {
                result.Success = false;
                result.ErrorMessage = "User or user role not found.";
                result.Code = 404;
            }
            else
            {
                result.Success = true;
            }

            return (roleName, result);
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.ErrorMessage = $"An unexpected error occurred: {ex.Message}";
            result.Code = 500;
            return (null, result);
        }
    }

    //public async Task<string? role, CommandExecutionResult Success>? GetRoleNameByUserId(string userid)
    //{
    //    var user = await _ApplicationDbContext.Users
    //        .FirstOrDefaultAsync(x => x.Id == userid && x.IsDeleted == false);
    //    var userRole = await _ApplicationDbContext.UserRoles.FirstOrDefaultAsync(x => x.UserId == userid);
    //    var role = await _ApplicationDbContext.Roles.FirstOrDefaultAsync(x => x.Id == userRole.RoleId);
    //    return (role.Name, new CommandExecutionResult() { Success == true});
    //}
}
