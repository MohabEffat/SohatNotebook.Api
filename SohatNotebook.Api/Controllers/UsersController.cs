using AutoMapper;
using DataService.Data;
using DataService.IConfiguration;
using Entities.DbSet;
using Entities.Dtos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace SohatNotebook.Api.Controllers
{

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsersController : BaseController
    {
        public UsersController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager, IMapper mapper) : base(unitOfWork, userManager, mapper)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _unitOfWork.Users.GetAllAsync();
            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(UserDto userDto)
        {
            var mappedUser = _mapper.Map<User>(userDto);

            await _unitOfWork.Users.AddAsync(mappedUser);
            await _unitOfWork.CompleteAsync();

            var response = new Response<User>();
            response.Content = mappedUser;

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetUser(Guid Id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(Id);
            var result = new Response<User>();

            if (user != null)
            {
                result.Content = user!;
                return Ok(user);
            }
            result.Error = PopulateError(404, "User Not Found", "Object Not Found");
            return BadRequest(result);


        }
    }
}
