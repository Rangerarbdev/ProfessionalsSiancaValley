using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProfessionalsSiancaValley.Api.Data;
using ProfessionalsSiancaValley.Api.Models;

namespace ProfessionalsSiancaValley.Api.Controllers
{
    [ApiController]
    [Route("api/admin/moderation")]
    public class AdminModerationController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdminModerationController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("miniatures-pendientes")]
        public async Task<IActionResult> GetPendientes()
        {
            var pendientes = await _context.Miniatures
                .Where(m => m.Bloqueado_Por_Sistema)
                .Include(m => m.Reports)
                .ToListAsync();

            return Ok(pendientes);
        }

        [HttpPut("aprobar/{id}")]
        public async Task<IActionResult> Aprobar(int id)
        {
            var miniature = await _context.Miniatures.FindAsync(id);
            if (miniature == null) return NotFound();

            miniature.Bloqueado_Por_Sistema = false;
            miniature.Estado_Revision = "Aprobado por Admin";
            miniature.TotalReportes = 0;

            await _context.SaveChangesAsync();

            return Ok("Miniatura aprobada.");
        }

        [HttpPut("rechazar/{id}")]
        public async Task<IActionResult> Rechazar(int id)
        {
            var miniature = await _context.Miniatures.FindAsync(id);
            if (miniature == null) return NotFound();

            miniature.Estado_Revision = "Rechazado por Admin";

            await _context.SaveChangesAsync();

            return Ok("Miniatura rechazada.");
        }
    }
}