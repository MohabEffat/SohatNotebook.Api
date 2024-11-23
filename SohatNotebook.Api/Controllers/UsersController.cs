using DataService.Data;
using Entities.DbSet;
using Entities.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace SohatNotebook.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;
        public UsersController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = _context.Users.Where(x => x.Status == 1).ToList();
            return Ok(users);
        }

        [HttpPost]
        public IActionResult AddUser(UserDto userDto)
        {
            var user = new User
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
                DateOfBirth = userDto.DateOfBirth,
                Country = userDto.Country,
                Phone = userDto.Phone,
                Status = 1
            };

            _context.Users.Add(user);
            _context.SaveChanges();
            return Ok();
        }

        [HttpGet("GetUser")]
        //[Route("GetUser")]
        public IActionResult GetUser(Guid Id)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == Id);

            return Ok(user);
        }
    }
}
