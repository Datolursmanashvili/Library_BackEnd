using Application.Shared;
using Domain.Entities.RoleEntity;
using Shared;

namespace Application.Queries.RoleQueries;

public class GetRoleForFiltersQuery : Query<GetRoleForFiltersQueryResult>
{
    public override async Task<QueryExecutionResult<GetRoleForFiltersQueryResult>> Execute()
    {
        var ApplicationRoles = _appContext.Roles.Where(x => x.IsDeleted == false).ToList();

        if (ApplicationRoles.IsNull()) return await Fail("თქვენ არ გაქვთ რეგისტრირებული როლი");


        //IEnumerable<Permissions> RemoveCostumPermisions = new List<Permissions>()
        //                        { (Permissions.UsersInWorkSchedulesRead),
        //                          (Permissions.UsersInWorkSchedulesWrite),
        //                          (Permissions.UsersInWorkSchedulesDelete),
        //                          (Permissions.PayrollModuleRead),
        //                          (Permissions.PayrollModuleWrite),
        //                          (Permissions.PayrollModuleDelete),
        //                          (Permissions.FullReportRead),
        //                          (Permissions.FullReportWrite),
        //                          (Permissions.FullReportDelete),
        //                          (Permissions.ReportsGovermentReportRead),
        //                          (Permissions.ReportsGovermentReportWrite),
        //                          (Permissions.ReportsGovermentReportDelete),
        //                          (Permissions.EditRecordsEWrite),
        //                          (Permissions.EditRecordDelete),
        //                          (Permissions.PersonalFullReport),
        //                          (Permissions.UsersWrite),
        //                          (Permissions.UsersDelete),
        //                        };

        //foreach (var perm in ApplicationRoles)
        //{
        //    var roleperm = perm.Permissions;
        //    if (roleperm.Count() > 0)
        //    {
        //        if (!roleperm.Contains(Permissions.UsersRead))
        //        {
        //            roleperm = roleperm.Except(RemoveCostumPermisions).ToList();
        //            perm.Permissions = roleperm;
        //            continue;
        //        }
        //    }
        //}

        var result = ApplicationRoles
             .Select(x => new GetRoleForFiltersResultItem
             {
                 RolePermissions = x.Permissions,
                 Id = x.Id,
                 Name = x.Name,
             })
             .ToList();


        return await Ok(new GetRoleForFiltersQueryResult() { Response = result });
    }
}
public class GetRoleForFiltersQueryResult
{
    public List<GetRoleForFiltersResultItem>? Response { get; set; }
}
public class GetRoleForFiltersResultItem
{
    public IEnumerable<Permissions>? RolePermissions { get; set; }
    public string Name { get; set; }
    public string Id { get; set; }
}