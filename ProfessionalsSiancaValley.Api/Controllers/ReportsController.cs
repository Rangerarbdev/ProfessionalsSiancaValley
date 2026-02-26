using Microsoft.AspNetCore.Mvc;
using ProfessionalsSiancaValley.Api.Data;
using ProfessionalsSiancaValley.Api.DTOs;
using ProfessionalsSiancaValley.Api.Models;
using ProfessionalsSiancaValley.Api.Helpers;
using Microsoft.EntityFrameworkCore;

namespace ProfessionalsSiancaValley.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReportsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateReportDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Descripcion))
                return BadRequest("La descripción es obligatoria.");

            var report = new Report
            {
                Id_Miniature = dto.Id_Miniature,
                Id_User = dto.Id_User,
                Nombre_Usuario = dto.Nombre_Usuario,
                Email_Usuario = dto.Email_Usuario,
                Titulo = dto.Titulo,
                Descripcion = dto.Descripcion
            };

            _context.Reports.Add(report);
            await _context.SaveChangesAsync();

            var miniature = await _context.Miniatures
    .       FirstOrDefaultAsync(m => m.Id_Miniature == report.Id_Miniature);

            if (miniature != null)
            {
                miniature.TotalReportes++;

                if (miniature.TotalReportes >= ModerationSettings.LIMITE_REPORTES)
                {
                    miniature.Bloqueado_Por_Sistema = true;
                    miniature.Estado_Revision = "Bloqueado Automaticamente";
                }

                await _context.SaveChangesAsync();
            }
            return Ok(report);
        }
    }
}
