using Application.Shared;
using Domain.Entities.RoleEntity;
using Microsoft.AspNetCore.Http;
using Shared;

namespace Application.Queries.RoleQueries
{
    public class EmployeePermissionsQuery : Query<EmployeePermissionsQueryResult>
    {
        public override async Task<QueryExecutionResult<EmployeePermissionsQueryResult>> Execute()
        {
            if (UserId.IsNull()) return await Fail(StatusCodes.Status401Unauthorized, "Anauthorized");

            List<Permissions> permissions = new List<Permissions>();

            var ApplicationRoles = ApplicationContext.UserRoles
                                                     .Where(x => x.UserId == UserId)
                                                     .Select(x => x.RoleId)
                                                     .ToList();
            if (ApplicationRoles.IsNotNull())
            {
                foreach (var RoleId in ApplicationRoles)
                {
                    var applicationRole = ApplicationContext.Set<ApplicationRole>()
                                                      .FirstOrDefault(x => x.Id == RoleId && x.IsDeleted == false);

                    if (applicationRole.Permissions.IsNotNull()) permissions.AddRange(applicationRole.Permissions.ToList());
                }
            }

            //var EmployeeCustomPermission = ApplicationContext.Set<EmployeeCustomPermission>()
            //                                        .FirstOrDefault(x => x.EmployeeId == UserId);
            //if (EmployeeCustomPermission.IsNotNull()) permissions.AddRange(EmployeeCustomPermission.Permissions.ToList());


            return await Ok(new EmployeePermissionsQueryResult() { Permissions = permissions });
        }
    }
    public class EmployeePermissionsQueryResult
    {
        public List<Permissions>? Permissions { get; set; }
    }
}
