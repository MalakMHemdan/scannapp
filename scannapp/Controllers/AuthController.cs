using BLL.DTOs;
using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace scannapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        
        // Register a new user
        
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(model);
            return Ok(new { Message = result });
        }

       
        // Login and get JWT token
       
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var token = await _authService.LoginAsync(model);
            if (token == null)
                return Unauthorized(new { Message = "Invalid username or password" });

            return Ok(new { Token = token });
        }

        
        // Get current logged-in user info
     
        [Authorize]
        [HttpGet("me")]
        public IActionResult GetCurrentUser()
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return Ok(new
            {
                UserId = userId,
                Username = username
            });
        }
    }
}
