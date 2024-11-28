using Authentication.Models.Dtos;
using DataService.IConfiguration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SohatNotebook.Api.Services.TokenService;

namespace SohatNotebook.Api.Controllers
{
    public class AccountController : BaseController
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenService _tokenService;

        public AccountController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager, ITokenService tokenService) : base(unitOfWork)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        [HttpPost]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequestDto input)
        {
            if (ModelState.IsValid)
            {
                var result = await _tokenService.verifyToken(input);

                if(result == null)
                {
                    return BadRequest(new RegisterResponseDto
                    {
                        Success = false,
                        Errors = new List<string>
                        {
                            "Token Validation Failed"
                        }
                    });
                }
                return Ok(result);
            }
            else
            {
                return BadRequest(new RegisterResponseDto
                {
                    Success = false,
                    Errors = new List<string> {
                        "Email Already Exists"
                    }
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody]RegisterDto input)
        {
            if (ModelState.IsValid)
            {
                var existUser = _userManager.FindByEmailAsync(input.Email);

                if (existUser == null)
                {
                    return BadRequest(new RegisterResponseDto
                    {
                        Success = false,
                        Errors = new List<string> {
                        "Email Already Exists"
                        }
                    });
                }
                var newUser = new IdentityUser
                {
                    Email = input.Email,
                    UserName = input.Email,
                    EmailConfirmed = true
                };

                var isCreated = await _userManager.CreateAsync(newUser, input.Password);

                if (!isCreated.Succeeded)
                {
                    return BadRequest(new RegisterResponseDto
                    {
                        Success = false,
                        Errors = isCreated.Errors.Select(x => x.Description).ToList()
                    });
                }

                var jwtToken = await _tokenService.GenerateTokenAsync(newUser);

                return Ok(new RegisterResponseDto
                {
                    Success = true,
                    Token = jwtToken.JwtToken,
                    RefreshToken = jwtToken.RefreshToken
                });
            }
            else
            {
                return BadRequest(new RegisterResponseDto
                {
                    Success = false,
                    Errors = new List<string> {
                        "Invalid Payload"
                    }
                });
            }
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto input)
        {
            if (ModelState.IsValid)
            {
                var existUser = await _userManager.FindByEmailAsync(input.Email);
                if (existUser == null)
                {
                    return BadRequest(new RegisterResponseDto
                    {
                        Success = false,
                        Errors = new List<string> {
                        "User Not Found"
                        }
                    });
                }
                var isCorrect = await _userManager.CheckPasswordAsync(existUser, input.Password);

                if (isCorrect)
                {
                    var jwtToken = await _tokenService.GenerateTokenAsync(existUser);
                    return Ok(new RegisterResponseDto
                    {
                        Success = true,
                        Token = jwtToken.JwtToken,
                        RefreshToken = jwtToken.RefreshToken
                    });
                }
                else
                {
                    return BadRequest(new RegisterResponseDto
                    {
                        Success = false,
                        Errors = new List<string> {
                        "Wrong Credentials"
                        }
                    });
                }
            }
            else
            {
                return BadRequest(new RegisterResponseDto
                {
                    Success = false,
                    Errors = new List<string> {
                        "Wrong Credentials"
                    }
                });
            }
        }
    }
}
