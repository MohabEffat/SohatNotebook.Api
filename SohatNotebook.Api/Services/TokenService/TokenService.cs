using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SohatNotebook.Authentication.Configuration.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SohatNotebook.Api.Services.TokenService
{
    public class TokenService : ITokenService
    {
        private readonly JwtOptions _options;
        public TokenService(IOptions<JwtOptions> options)
        {
            _options = options.Value;
        }
        public async Task<string> GenerateTokenAsync(IdentityUser user)
        {

            var jwtHandler = new JwtSecurityTokenHandler();

            var key = new SymmetricSecurityKey (Encoding.UTF8.GetBytes(_options.Key));

            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim("Id", user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var userCreds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
               Subject = new ClaimsIdentity(userClaims),
               Expires = DateTime.UtcNow.AddHours(3),
               SigningCredentials = userCreds
            };
            var token = jwtHandler.CreateToken(tokenDescriptor);

            var jwtToken = jwtHandler.WriteToken(token);

            return jwtToken;
        }
    }
}
