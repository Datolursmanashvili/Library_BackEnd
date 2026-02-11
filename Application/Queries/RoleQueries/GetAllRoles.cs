using Application.Shared;
using Shared;

namespace Application.Queries.RoleQueries
{
    public class GetAllRoles : Query<List<RoleItemResponseItem>?>
    {
        public override async Task<QueryExecutionResult<List<RoleItemResponseItem>?>> Execute()
        {
            return await Ok(_appContext.Roles.Select(x => new RoleItemResponseItem()
            {
                Id = x.Id,
                Name = x.Name,
                NormaliseName = x.NormalizedName
            })?.ToList());
        }
    }
    public class RoleItemResponseItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string NormaliseName { get; set; }
    }
}
