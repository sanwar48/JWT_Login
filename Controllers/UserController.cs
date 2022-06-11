using AuthProject.Models;
using AuthProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthProject.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService _userService)
        {
            userService = _userService;
        }
        // GET: api/<UserController>
        [HttpGet]

        public ActionResult<List<User>> GetUsers()
        {
            return userService.GetUsers();
        }

        [HttpGet("{id}")]

        public ActionResult<User>GetUser(string id)
        {
            return userService.GetUser(id);
        }

        [HttpPost]

        public ActionResult<User>Create([FromBody] User newUser)
        {
            userService.Create(newUser);

            return CreatedAtAction(nameof(GetUser), new { id = newUser.Id }, newUser);
        }


        [AllowAnonymous]
        [Route("authenticate")]
        [HttpPost]

        public ActionResult Login([FromBody] User user)
        {
            var token = userService.Authenticate(user.Email, user.Password);

            if(token==null)
                return Unauthorized();

            return Ok(new { token, user });
    }
    }

    

}
