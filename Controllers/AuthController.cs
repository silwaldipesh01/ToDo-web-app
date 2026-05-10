using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDo_App.Model;
using ToDo_App.Model.DTO;
using ToDo_App.Services;
using ToDo_App.Services.Interfaces;

namespace ToDo_App.Controllers
{
    [Route("ToDo/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO login)
        {
            if (_authService.ValidateUser(login.Username, login.Password))
            {
                User user = new User { Username = login.Username, Role = "User" };

                var token = _authService.GenerateJwtToken(user);
                return Ok(new { token });
               // return Ok(new {message="login sucessfull"});
            }

            return Unauthorized("Invalid credentials");
        }
    }

    public class LoginDTO
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}