using Authentication.Models.Dtos;
using Microsoft.AspNetCore.Identity;

namespace SohatNotebook.Api.Services.TokenService
{
    public interface ITokenService
    {
        public Task<TokenData> GenerateTokenAsync(IdentityUser user);
        public Task<AuthResultDto> verifyToken(TokenRequestDto input);
    }
}
