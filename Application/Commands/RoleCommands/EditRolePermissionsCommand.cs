using Application.Shared;
using Domain.Entities.RoleEntity;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Shared;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Commands.RoleCommands
{
    public class EditRolePermissionsCommand : Command
    {
        public string? Id { get; set; }
        public IEnumerable<Permissions>? RolePermissions { get; set; }

        public override async Task<RepositoryExecutionResult> ExecuteAsync()
        {
            var role = applicationDbContext.Set<ApplicationRole>().FirstOrDefault(x => x.Id == Id && !x.IsDeleted);

            var UserRole = applicationDbContext.UserRoles.Where(x => x.RoleId == Id);
            #region old code

            //Permissions userReadPermision = Permissions.UsersRead;


            //if (UserRole.IsNotNull())
            //{
            //    var usersID = UserRole.Select(x => x.UserId).ToList();
            //    //var EmployeeCostumPermission = ApplicationDbContext.EmployeeCustomPermissions.Where(x => usersID.Contains(x.EmployeeId) && x.IsDeleted == false);
            //    if (EmployeeCostumPermission.IsNotNull())
            //    {
            //        foreach (var userCostumPermision in EmployeeCostumPermission)
            //        {
            //            if (userCostumPermision.EmployeeId == "e1044e7b-dbdc-46ac-a004-9c29b8dffd4d")
            //            {

            //            }
            //            var CustumePermision = userCostumPermision.Permissions;
            //            if (CustumePermision.IsNotNull())
            //            {
            //                if (!RolePermissions.Contains(Permissions.UsersRead) && CustumePermision.Contains(Permissions.UsersRead) ||
            //                    !RolePermissions.Contains(Permissions.UsersDelete) && CustumePermision.Contains(Permissions.UsersDelete) ||
            //                    !RolePermissions.Contains(Permissions.UsersWrite) && CustumePermision.Contains(Permissions.UsersWrite))
            //                {
            //                    IEnumerable<Permissions> RemoveCostumPermisions = new List<Permissions>()
            //                    { (Permissions.UsersInWorkSchedulesRead),
            //                      (Permissions.UsersInWorkSchedulesWrite),
            //                      (Permissions.UsersInWorkSchedulesDelete),
            //                      (Permissions.PayrollModuleRead),
            //                      (Permissions.PayrollModuleWrite),
            //                      (Permissions.PayrollModuleDelete),
            //                      (Permissions.FullReportRead),
            //                      (Permissions.FullReportWrite),
            //                      (Permissions.FullReportDelete),
            //                      (Permissions.ReportsGovermentReportRead),
            //                      (Permissions.ReportsGovermentReportWrite),
            //                      (Permissions.ReportsGovermentReportDelete),
            //                      (Permissions.EditRecordsEWrite),
            //                      (Permissions.EditRecordDelete),
            //                      (Permissions.PersonalFullReport),
            //                    };

            //                    foreach (var perm in RemoveCostumPermisions)
            //                    {
            //                        if (CustumePermision.Contains(perm))
            //                        {
            //                            CustumePermision.ToList().Remove(perm);
            //                        }
            //                    }


            //                }
            //                //EmployeeCustomPermissionRepository.EditEmployeeCustomPermission(userCostumPermision, userCostumPermision.EmployeeId, CustumePermision);
            //            }
            //        }
            //    }
            //}
            #endregion
            if (role.IsNull())
            {
                return await Fail(StatusCodes.Status404NotFound, "ასეთი_როლი_ვერ_მოიძებნა");//"ასეთი როლი ვერ მოიძებნა
            }

            return await RoleRepository.EditRolePermissions(role, RolePermissions);
        }
    }
    public class EditRolePermissionsCommandValidation : AbstractValidator<EditRolePermissionsCommand>
    {
        public EditRolePermissionsCommandValidation()
        {
            RuleFor(x => x.RolePermissions).Must(x => x.Count() > 0);
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
