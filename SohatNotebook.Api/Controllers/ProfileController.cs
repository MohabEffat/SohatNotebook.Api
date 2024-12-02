using DataService.IConfiguration;
using Entities.Dtos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace SohatNotebook.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProfileController : BaseController
    {

        public ProfileController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager) : base(unitOfWork, userManager)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (currentUser == null)
                return BadRequest("User Not Found");

            var profile = await _unitOfWork.Users.GetByIdentityIdAsync(new Guid(currentUser.Id));

            if (profile == null)
                return BadRequest("User Not Found");


            return Ok(profile);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UpdateProfileDto input)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid Payload");

            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (currentUser == null)
                return BadRequest("User Not Found");

            var profile = await _unitOfWork.Users.GetByIdentityIdAsync(new Guid(currentUser.Id));

            if (profile == null)
                return BadRequest("User Not Found");

            profile.Address = input.Address;
            profile.FirstName = input.FirstName;
            profile.LastName = input.LastName;
            profile.Country = input.Country;
            profile.PhoneNumber = input.PhoneNumber;

            var isUpdated = await _unitOfWork.Users.UpdateUserProfileAsync(profile);

            if (isUpdated)
            {
                await _unitOfWork.CompleteAsync();
                return Ok(profile);
            }

            return BadRequest("Error, Try Again");

        }

    }
}
