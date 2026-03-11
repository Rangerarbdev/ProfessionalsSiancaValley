using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        // ===============================
        // CREAR MINIATURA
        // POST api/miniatures
        // ===============================
        [HttpPost]
        public async Task<IActionResult> Create(CreateMiniatureDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Id_User))
                return BadRequest("Id_User es obligatorio.");

            if (string.IsNullOrWhiteSpace(dto.Titulo))
                return BadRequest("El título es obligatorio.");

            if (string.IsNullOrWhiteSpace(dto.Descripcion))
                return BadRequest("La descripción es obligatoria.");

            if (string.IsNullOrWhiteSpace(dto.Url_Miniatura))
                return BadRequest("La URL de la miniatura es obligatoria.");

            // Verificar que el usuario exista
            var userExists = await _context.Users
                .AnyAsync(u => u.IdUser == dto.Id_User);

            if (!userExists)
                return BadRequest("El usuario no existe.");

            var miniature = new Miniature
            {
                Id_Publicacion = dto.Id_Publicacion,
                Id_User = dto.Id_User,
                Nombre_Usuario = dto.Nombre_Usuario,
                Email_Usuario = dto.Email_Usuario,
                Tipo_Contenido = dto.Tipo_Contenido,
                Url_Miniatura = dto.Url_Miniatura,
                Titulo = dto.Titulo,
                Descripcion = dto.Descripcion,
                CreatedAt = DateTime.UtcNow
            };

            _context.Miniatures.Add(miniature);

            await _context.SaveChangesAsync();

            return Created("", new
            {
                message = "Miniatura creada correctamente",
                miniature
            });
        }
    }
}