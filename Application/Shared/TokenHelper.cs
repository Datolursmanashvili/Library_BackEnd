using Domain.Entities.RoleEntity;
using Domain.Entities.UserEntity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Shared
{
    public static class TokenHelper
    {

        public static TokenResult GenerateToken(User user, ApplicationRole applicationRole)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var claims = new Dictionary<string, string>()
        {
            { "email_address", user.Email },
            { "UserName", user.UserName },
            { "UserId", user.Id },
            { "role", applicationRole.NormalizedName },
            { "FirstName", user.FirstName.ToString()},
            { "LastName", user.LastName.ToString()},
            { "DepartmentId", user.DepartmentId.ToString()},

  };

            var accessToken = GenerateAccessToken(user.Id, claims);
            return accessToken;
        }

        public static string GenerateToken(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var claims = new Dictionary<string, string>()
        {
            { "email_address", user.Email },
            { "UserName", user.UserName },
            { "UserId", user.Id },
        };

            var accessToken = GenerateAccessToken(user.Id, claims);
            return accessToken.Token;
        }

        private static TokenResult GenerateAccessToken(string id, IDictionary<string, string> claims)
        {
            var convertedClaims = claims?.Select(x => new Claim(x.Key, x.Value)).ToList() ?? new List<Claim>();
            convertedClaims.Add(new Claim(JwtRegisteredClaimNames.Sub, id.ToString()));
            convertedClaims.Add(new Claim(ClaimTypes.Name, id.ToString()));

            var accessToken = GenerateJwt(convertedClaims);
            var tokenResponse = new JwtSecurityTokenHandler().WriteToken(accessToken);

            return new TokenResult()
            {
                Token = tokenResponse,
                Expirtaion = accessToken.ValidTo
            };
        }

        private static JwtSecurityToken GenerateJwt(IEnumerable<Claim> claims)
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("B374A26A71490437AA024E4FADD5B497FDFF1A8EA6FF12F6FB65AF2720B59CCF"));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: "Issuer",
                audience: "Audience",
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddHours(10),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }
    }


}
public class TokenResult
{
    public string Token { get; set; }
    public DateTime Expirtaion { get; set; }
}