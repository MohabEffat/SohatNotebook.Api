using Microsoft.AspNetCore.Identity;

namespace SohatNotebook.Api.Services.TokenService
{
    public interface ITokenService
    {
        public string GenerateTokenAsync(IdentityUser user);
    }
}
