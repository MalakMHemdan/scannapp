using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace scannapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;

        public RoleController(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        
        [HttpGet("all")]
        public IActionResult GetRoles()
        {
            return Ok(_roleManager.Roles.Select(r => r.Name).ToList());
        }

        
        [HttpPost("create")]
        public async Task<IActionResult> CreateRole([FromQuery] string roleName)
        {
            if (await _roleManager.RoleExistsAsync(roleName))
                return BadRequest("Role already exists");

            await _roleManager.CreateAsync(new IdentityRole<int>(roleName));
            return Ok($"Role {roleName} created successfully");
        }

        
        [HttpPost("assign")]
        public async Task<IActionResult> AssignRole([FromQuery] string username, [FromQuery] string roleName)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return NotFound("User not found");

            if (!await _roleManager.RoleExistsAsync(roleName))
                return BadRequest("Role not found");

            await _userManager.AddToRoleAsync(user, roleName);
            return Ok($"Role {roleName} assigned to {username}");
        }

        
        [HttpPost("remove")]
        public async Task<IActionResult> RemoveRole([FromQuery] string username, [FromQuery] string roleName)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return NotFound("User not found");

            if (!await _roleManager.RoleExistsAsync(roleName))
                return BadRequest("Role not found");

            await _userManager.RemoveFromRoleAsync(user, roleName);
            return Ok($"Role {roleName} removed from {username}");
        }
    }
}
