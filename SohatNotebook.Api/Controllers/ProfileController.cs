using AutoMapper;
using DataService.IConfiguration;
using Entities.DbSet;
using Entities.Dtos;
using Entities.Dtos.Errors;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace SohatNotebook.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProfileController : BaseController
    {

        public ProfileController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager, IMapper mapper) : base(unitOfWork, userManager, mapper)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            var response = new Response<UserDto>();

            if (currentUser == null)
            {
                response.Error = PopulateError(400, "User Not Found", "Bad Requset");
                return BadRequest(response);
            }

            var profile = await _unitOfWork.Users.GetByIdentityIdAsync(new Guid(currentUser.Id));

            if (profile == null)
            {
                response.Error = PopulateError(400, "User Not Found", "Bad Requset");
                return BadRequest(response);
            }
            var mappedUser = _mapper.Map<UserDto>(profile);
            response.Content = mappedUser;

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UpdateProfileDto input)
        {
            var response = new Response<ProfileDto>();

            if (!ModelState.IsValid)
            {
                response.Error = PopulateError(400, "User Not Found", "Bad Requset");
                return BadRequest(response);
            }

            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (currentUser == null)
            {
                response.Error = PopulateError(400, "User Not Found", "Bad Requset");
                return BadRequest(response);
            }

            var profile = await _unitOfWork.Users.GetByIdentityIdAsync(new Guid(currentUser.Id));

            if (profile == null)
            {
                response.Error = PopulateError(400, "User Not Found", "Bad Requset");
                return BadRequest(response);
            }

            var isUpdated = await _unitOfWork.Users.UpdateUserProfileAsync(profile);

            if (isUpdated)
            {
                await _unitOfWork.CompleteAsync();

                var mappedProfile = _mapper.Map<ProfileDto>(profile);

                response.Content = mappedProfile;

                return Ok(response);
            }

            response.Error = PopulateError(500, "Something went wrong, please try again later", "Unable to process the request");

            return BadRequest(response);

        }

    }
}
