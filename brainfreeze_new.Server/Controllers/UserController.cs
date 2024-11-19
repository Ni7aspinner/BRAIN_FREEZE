using brainfreeze_new.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace brainfreeze_new.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return BadRequest("Username cannot be empty.");
            }

            _userService.StoreUsername(username);

            return Ok(new { message = "User logged in successfully" });
        }

        [HttpGet("info")]
        public IActionResult GetUserInfo()
        {
            var userInfo = _userService.GetUserInfo();

            if (userInfo == null)
            {
                return NotFound("User not found.");
            }

            return Ok(userInfo);
        }
    }
}
