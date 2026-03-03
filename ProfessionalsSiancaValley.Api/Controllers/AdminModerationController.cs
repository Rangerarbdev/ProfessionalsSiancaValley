using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ProfessionalsSiancaValley.Api.Controllers
{
    [ApiController]
    [Route("api/admin/moderation")]
    public class AdminModerationController : ControllerBase
    {
        [Authorize(Roles = "Admin")]
        [HttpGet("miniatures-pendientes")]
        public IActionResult GetPendientes()
        {
            return Ok(new
            {
                IsAuthenticated = User.Identity?.IsAuthenticated,
                Role = User.FindFirst(ClaimTypes.Role)?.Value
            });
        }
    }
}