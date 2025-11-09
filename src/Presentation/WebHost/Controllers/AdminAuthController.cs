using Microsoft.AspNetCore.Mvc;
using QuizeMC.Application.Models.Admin;
using QuizeMC.Application.Services.Abstractions;

namespace QuizeMC.Presentation.WebHost.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminAuthController : ControllerBase
    {
        private readonly IAdminApplicationService _adminService;

        public AdminAuthController(IAdminApplicationService adminService)
        {
            _adminService = adminService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AdminCreateModel model)
        {
            var result = await _adminService.RegisterAsync(model);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AdminLoginModel model)
        {
            var result = await _adminService.LoginAsync(model);

            if (!result.Success)
                return Unauthorized(result);

            return Ok(result);
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel model)
        {
            // Get admin ID from JWT token (will be implemented later)
            var adminId = GetCurrentAdminId();

            var result = await _adminService.ChangePasswordAsync(
                adminId, model.CurrentPassword, model.NewPassword);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        private Guid GetCurrentAdminId()
        {
            // Temporary implementation - will be replaced with JWT claims
            return Guid.Parse("00000000-0000-0000-0000-000000000001");
        }
    }

    public class ChangePasswordModel
    {
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}