using Microsoft.AspNetCore.Identity;

namespace SohatNotebook.Api.Services.TokenService
{
    public interface ITokenService
    {
        public Task<string> GenerateTokenAsync(IdentityUser user);
    }
}
