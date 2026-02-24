using Microsoft.AspNetCore.Mvc;
using ProfessionalsSiancaValley.Api.Data;
using ProfessionalsSiancaValley.Api.DTOs;
using ProfessionalsSiancaValley.Api.Models;

namespace ProfessionalsSiancaValley.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MiniaturesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MiniaturesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateMiniatureDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Descripcion))
                return BadRequest("La descripción es obligatoria.");

            var miniature = new Miniature
            {
                Id_Publicacion = dto.Id_Publicacion,
                Id_User = dto.Id_User,
                Nombre_Usuario = dto.Nombre_Usuario,
                Email_Usuario = dto.Email_Usuario,
                Tipo_Contenido = dto.Tipo_Contenido,
                Url_Miniatura = dto.Url_Miniatura,
                Titulo = dto.Titulo,
                Descripcion = dto.Descripcion
            };

            _context.Miniatures.Add(miniature);
            await _context.SaveChangesAsync();

            return Ok(miniature);
        }
    }
}
