using Microsoft.AspNetCore.Authorization;
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

        // ==========================================
        // CREAR MINIATURA
        // POST api/miniatures
        // ==========================================
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateMiniatureDto dto)
        {
            var idUser = User.FindFirst("IdUser")?.Value;

            if (idUser == null)
                return Unauthorized("Token inválido.");

            if (string.IsNullOrWhiteSpace(dto.Titulo))
                return BadRequest("El título es obligatorio.");

            if (string.IsNullOrWhiteSpace(dto.Descripcion))
                return BadRequest("La descripción es obligatoria.");

            if (string.IsNullOrWhiteSpace(dto.Url_Miniatura))
                return BadRequest("La URL de la miniatura es obligatoria.");

            if (string.IsNullOrWhiteSpace(dto.Id_Publicacion))
                return BadRequest("Id_Publicacion es obligatorio.");

            // Obtener usuario desde DB
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.IdUser == idUser);

            if (user == null)
                return BadRequest("El usuario no existe.");

            var miniature = new Miniature
            {
                Id_Publicacion = dto.Id_Publicacion,
                Id_User = user.IdUser,
                UserPosition = user.UserPosition,
                Nombre_Usuario = $"{user.FirstName} {user.LastName}",
                Email_Usuario = user.Email,
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

        // ==========================================
        // OBTENER TODAS LAS MINIATURAS
        // GET api/miniatures
        // ==========================================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var miniatures = await _context.Miniatures
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();

            return Ok(miniatures);
        }

        // ==========================================
        // OBTENER MINIATURAS POR USUARIO
        // GET api/miniatures/user/{id}
        // ==========================================
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetByUser(string id)
        {
            var miniatures = await _context.Miniatures
                .Where(m => m.Id_User == id)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();

            return Ok(miniatures);
        }

        // ==========================================
        // ELIMINAR MINIATURA
        // DELETE api/miniatures/{id}
        // ==========================================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var miniature = await _context.Miniatures.FindAsync(id);

            if (miniature == null)
                return NotFound("Miniatura no encontrada.");

            _context.Miniatures.Remove(miniature);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Miniatura eliminada correctamente"
            });
        }

        // ==========================================
        // FEED PRINCIPAL (Publicaciones + Media)
        // GET api/miniatures/feed
        // ==========================================
        [HttpGet("feed")]
        public async Task<IActionResult> GetFeed()
        {
            var data = await _context.Publications
                .Join(_context.MediaFiles,
                      p => p.Id_Publicacion,
                      m => m.Id_Publicacion,
                      (p, m) => new
                      {
                          p.Id_Publicacion,
                          p.Titulo,
                          p.Descripcion,
                          p.CreatedAt,
                          p.Vistas,
                          p.Likes,
                          p.Dislikes,
                          m.UrlArchivo,
                          m.Tipo_Contenido
                      })
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            return Ok(data);
        }
    }
}