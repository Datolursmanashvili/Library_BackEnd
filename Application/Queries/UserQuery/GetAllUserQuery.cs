using Application.Shared;
using Shared;

namespace Application.Queries.UserQuery
{
    public class GetAllUserQuery : Query<GetAllUserQueryResult>
    {
        public int PageSize { get; set; }
        public int Page { get; set; }
        public override async Task<QueryExecutionResult<GetAllUserQueryResult>> Execute()
        {

            var users = _appContext.Users.AsQueryable();
            var totalcount = users.Count();

            var result = users?.Select(x => new UserQueryResultItem()
            {
                Id = x.Id,
                Email = x.Email,
                //IsActive = x.IsActive,
                FirstName = x.FirstName,
                LastName = x.LastName,
                PNumber = userRepository.Decrypt(x.PNumber),
                Phone = x.PhoneNumber,
                Username = x.UserName,
                DepartmentId = x.DepartmentId,

                BirthDate = x.BirthDate,

            })?.Skip(Page * PageSize)
               .Take(PageSize)
               .ToList();


            var response = new GetAllUserQueryResult();
            response.Result = result;
            response.TotalCount = totalcount;
            return await Ok(response);
        }
    }

    public class UserQueryResultItem
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public int DepartmentId { get; set; }
        public DateTime BirthDate { get; set; }
    }

    public class GetAllUserQueryResult
    {
        public List<UserQueryResultItem>? Result { get; set; }
        public int? TotalCount { get; set; }
    }
}
