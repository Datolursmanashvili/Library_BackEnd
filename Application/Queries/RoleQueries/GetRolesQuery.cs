using Application.Shared;
using Domain.Entities.RoleEntity;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Application.Queries.RoleQueries
{
    public class GetRolesQuery : Query<GetRolesQueryResult>
    {
        public string? Purpose { get; set; }

        public override async Task<QueryExecutionResult<GetRolesQueryResult>> Execute()
        {
            var ApplicationRoles = Purpose == null ? await ApplicationContext.Set<ApplicationRole>()
                                                                             .Where(x => !x.IsDeleted)
                                                                             .OrderByDescending(x => x.CreatedAt)
                                                                             .ToListAsync() :
                                                     await ApplicationContext.Set<ApplicationRole>()
                                                                             .Where(x => x.Name.Contains(Purpose) && !x.IsDeleted)
                                                                             .OrderByDescending(x => x.CreatedAt)
                                                                             .ToListAsync();


            var result = ApplicationRoles
                .Select(x => new GetRolesQueryResultItem
                {
                    Id = x.Id,
                    Name = x.Name,
                    Permissions = x.Permissions
                })
                .ToList();

            return await Ok(new GetRolesQueryResult() { Response = result });
        }
    }

    public class GetRolesQueryResult
    {
        public List<GetRolesQueryResultItem> Response { get; set; }
    }
    public class GetRolesQueryResultItem
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public IEnumerable<Permissions>? Permissions { get; set; }
    }
}
