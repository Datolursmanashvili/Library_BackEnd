using Application.Shared;
using Shared;
using static Application.Queries.UserQuery.LoginQuery;

namespace Application.Queries.UserQuery
{
    public class LoginQuery : Query<LoginQueryResult>
    {
        public string Password { get; set; }
        public string Email { get; set; }

        public override async Task<QueryExecutionResult<LoginQueryResult>> Execute()
        {
            if (!PasswordHelper.IsValidEmail(Email))
            {
                return await Fail("არასწორი მეილი");
            }

            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
            {
                return await Fail("მომხმარებელი ვერ მოიძებნა");
            }
            if (!user.IsActive)
            {
                return await Fail("მომხმარებელი არ არის აქტიური");
            }


            if (await _userManager.CheckPasswordAsync(user, Password))
            {
                var roleID = _appContext.UserRoles.FirstOrDefault(x => x.UserId == user.Id).RoleId;

                var Role = _appContext.Roles.FirstOrDefault(x => x.Id == roleID && x.IsDeleted == false);

                if (Role.IsNull())
                {
                    return await Fail("role not found");
                }
                var tokenResult = TokenHelper.GenerateToken(user, Role);
                return await Ok(new LoginQueryResult
                {
                    Token = tokenResult.Token,
                    Expirtaion = tokenResult.Expirtaion,
                    DepartmentId = user.DepartmentId,
                });
            }
            return await Fail("პაროლი არასწორია");
        }


        public class LoginQueryResult
        {
            public string Token { get; set; }
            public int? DepartmentId { get; set; }
            public DateTime Expirtaion { get; set; }
        }
    }
}
