using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SohatNotebook.Authentication.Configuration.Options;
using System.Text;

namespace SohatNotebook.Api.Configuration.OptionsSetup
{
    public class JwtBearerOptionsSetup : IConfigureOptions<JwtBearerOptions>
    {
        private readonly JwtOptions _jwtOptions;

        public JwtBearerOptionsSetup(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }


        public void Configure(JwtBearerOptions options)
        {
            options.SaveToken = true;
            options.TokenValidationParameters = new()
            {
                ValidateIssuer = false,
                //ValidIssuer = _jwtOptions.Issuer,
                ValidateAudience = false,
                //ValidAudience = _jwtOptions.Audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key)),
                RequireExpirationTime = false,
                ValidateLifetime = true,
            };
        }
    }
}
