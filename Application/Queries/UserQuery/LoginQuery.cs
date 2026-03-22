using Application.Shared;
using Microsoft.AspNetCore.Http;
using Shared;
using static Application.Queries.UserQuery.LoginQuery;

namespace Application.Queries.UserQuery
{
    /// <summary>
    /// Credentials for <c>User/Login</c> (GET query string).
    /// </summary>
    public class LoginQuery : Query<LoginQueryResult>
    {
        /// <summary>Plain-text password.</summary>
        public string Password { get; set; }

        /// <summary>User email (used as login identifier).</summary>
        public string Email { get; set; }

        /// <inheritdoc />
        public override async Task<QueryExecutionResult<LoginQueryResult>> Execute()
        {
            if (!PasswordHelper.IsValidEmail(Email))
            {
                return await Fail(StatusCodes.Status400BadRequest, "არასწორი მეილი");
            }

            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
            {
                return await Fail(StatusCodes.Status401Unauthorized, "მომხმარებელი ვერ მოიძებნა");
            }
            if (!user.IsActive)
            {
                return await Fail(StatusCodes.Status403Forbidden, "მომხმარებელი არ არის აქტიური");
            }


            if (await _userManager.CheckPasswordAsync(user, Password))
            {
                var roleID = _appContext.UserRoles.FirstOrDefault(x => x.UserId == user.Id).RoleId;

                var Role = _appContext.Roles.FirstOrDefault(x => x.Id == roleID && x.IsDeleted == false);

                if (Role.IsNull())
                {
                    return await Fail(StatusCodes.Status404NotFound, "role not found");
                }
                var tokenResult = TokenHelper.GenerateToken(user, Role);
                return await Ok(new LoginQueryResult
                {
                    Token = tokenResult.Token,
                    Expirtaion = tokenResult.Expirtaion,
                    DepartmentId = user.DepartmentId,
                });
            }
            return await Fail(StatusCodes.Status401Unauthorized, "პაროლი არასწორია");
        }


        /// <summary>Successful login payload (JWT and metadata).</summary>
        public class LoginQueryResult
        {
            /// <summary>Bearer token string.</summary>
            public string Token { get; set; }

            /// <summary>User department identifier when applicable.</summary>
            public int? DepartmentId { get; set; }

            /// <summary>Token expiry (UTC).</summary>
            public DateTime Expirtaion { get; set; }
        }
    }
}
