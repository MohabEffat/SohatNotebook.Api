using Authentication.Models.Dtos;
using DataService.IConfiguration;
using Entities.DbSet;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly TokenValidationParameters _validationParameters;
        private readonly UserManager<IdentityUser> _userManager;
        public TokenService(IOptions<JwtOptions> options,
            TokenValidationParameters validationParameters, IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _options = options.Value;
            _validationParameters = validationParameters;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public async Task<TokenData> GenerateTokenAsync(IdentityUser user)
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
               Expires = DateTime.UtcNow.Add(_options.ExpiryTimeFrame),
               SigningCredentials = userCreds
            };
            var token = jwtHandler.CreateToken(tokenDescriptor);

            var jwtToken = jwtHandler.WriteToken(token);

            var refreshToken = new RefreshToken
            {
                AddDate = DateTime.UtcNow,
                Token = $"{GenerateRandomString(25)}_{Guid.NewGuid()}",
                UserId = user.Id,
                IsRevoked = false,
                IsUsed = false,
                Status = 1,
                JwtId = token.Id,
                ExpiryDate = DateTime.UtcNow.AddMonths(6),
            };
            await _unitOfWork.RefreshTokens.AddAsync(refreshToken);
            await _unitOfWork.CompleteAsync();

            var tokenData = new TokenData
            {
                JwtToken = jwtToken,
                RefreshToken = refreshToken.Token
            };

            return tokenData;
        }

        private string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var stringChars = new char[length];

            for (int i = 0; i < length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return new string(stringChars);
        }

        public async Task<AuthResultDto> verifyToken(TokenRequestDto input)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                Console.WriteLine($"Verifying Refresh Token: {input.RefreshToken}");

                var principal = tokenHandler.ValidateToken(input.RefreshToken, _validationParameters, out var validatedToken);

                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var isValidAlgorithm = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
                    if (!isValidAlgorithm)
                    {
                        return new AuthResultDto
                        {
                            Success = false,
                            Errors = new List<string> { "Invalid token algorithm." }
                        };
                    }
                }

                var expClaim = principal.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp)?.Value;
                if (string.IsNullOrEmpty(expClaim) || !long.TryParse(expClaim, out var utcExpiryDate))
                {
                    return new AuthResultDto
                    {
                        Success = false,
                        Errors = new List<string> { "Invalid token expiration claim." }
                    };
                }

                var expDate = UnixTimeStampToDateTime(utcExpiryDate);
                if (expDate > DateTime.UtcNow)
                {
                    return new AuthResultDto
                    {
                        Success = false,
                        Errors = new List<string> { "JWT token has not expired." }
                    };
                }

                var refreshTokenExist = await _unitOfWork.RefreshTokens.GetByRefreshToken(input.RefreshToken);
                if (refreshTokenExist == null || refreshTokenExist.IsRevoked || refreshTokenExist.IsUsed)
                {
                    return new AuthResultDto
                    {
                        Success = false,
                        Errors = new List<string> { "Invalid or reused refresh token." }
                    };
                }

                var jti = principal.Claims.SingleOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)?.Value;
                if (refreshTokenExist.JwtId != jti)
                {
                    return new AuthResultDto
                    {
                        Success = false,
                        Errors = new List<string> { "Token mismatch." }
                    };
                }

                refreshTokenExist.IsUsed = true;
                await _unitOfWork.RefreshTokens.MarkRefreshTokenAsUsed(refreshTokenExist);
                await _unitOfWork.CompleteAsync();

                var dbUser = await _userManager.FindByIdAsync(refreshTokenExist.UserId);
                if (dbUser == null)
                {
                    return new AuthResultDto
                    {
                        Success = false,
                        Errors = new List<string> { "User not found." }
                    };
                }

                var token = await GenerateTokenAsync(dbUser);

                return new AuthResultDto
                {
                    Success = true,
                    Token = token.JwtToken,
                    RefreshToken = token.RefreshToken
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new AuthResultDto
                {
                    Success = false,
                    Errors = new List<string> { "An error occurred during token verification." }
                };
            }
        }

        private DateTime UnixTimeStampToDateTime (long unixDate)
        {
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixDate).ToUniversalTime();
            return dateTime;
        }
    }
}
