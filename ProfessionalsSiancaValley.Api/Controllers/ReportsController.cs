using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProfessionalsSiancaValley.Api.Data;
using ProfessionalsSiancaValley.Api.DTOs;
using ProfessionalsSiancaValley.Api.Helpers;
using ProfessionalsSiancaValley.Api.Models;

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

        // ==========================================
        // 1️⃣ REPORTES SOBRE MINIATURES (LEGACY)
        // ==========================================
        [HttpPost("miniature")]
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
                Descripcion = dto.Descripcion,
                Fecha_Reporte = DateTime.UtcNow
            };

            _context.Reports.Add(report);
            await _context.SaveChangesAsync();

            var miniature = await _context.Miniatures
                .FirstOrDefaultAsync(m => m.Id_Miniature == report.Id_Miniature);

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

        // ==========================================
        // 2️⃣ REPORTES SOBRE PUBLICATIONS (NUEVO PRO)
        // ==========================================
        [Authorize]
        [HttpPost("{idPublicacion}")]
        public async Task<IActionResult> Report(string idPublicacion)
        {
            var idUser = User.FindFirst("IdUser")?.Value;

            if (idUser == null)
                return Unauthorized();

            var pub = await _context.Publications
                .FirstOrDefaultAsync(p => p.Id_Publicacion == idPublicacion);

            if (pub == null)
                return NotFound();

            // 🚫 EVITAR DOBLE REPORTE
            var existe = await _context.Reports.AnyAsync(r =>
                r.Id_Publicacion == idPublicacion &&
                r.Id_User == idUser);

            if (existe)
                return Ok(new { message = "Ya reportaste este contenido" });

            // 🆕 CREAR REPORTE
            var report = new Report
            {
                Id_Publicacion = idPublicacion,
                Id_User = idUser,
                Fecha_Reporte = DateTime.UtcNow
            };

            _context.Reports.Add(report);

            // 📊 CONTADOR
            pub.TotalReportes++;

            // 🚨 BLOQUEO AUTOMÁTICO
            if (pub.Dislikes >= 20 || pub.TotalReportes >= 5)
            {
                pub.Bloqueado_Por_Sistema = true;
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Reporte enviado 🚨",
                pub.TotalReportes,
                pub.Bloqueado_Por_Sistema
            });
        }
    }
}