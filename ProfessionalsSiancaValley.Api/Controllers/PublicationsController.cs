using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProfessionalsSiancaValley.Api.Data;
using ProfessionalsSiancaValley.Api.DTOs;
using ProfessionalsSiancaValley.Api.Models;
using ProfessionalsSiancaValley.Api.Services;

namespace ProfessionalsSiancaValley.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicationsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly PublicationService _service;

        public PublicationsController(AppDbContext context,
                                      PublicationService service)
        {
            _context = context;
            _service = service;
        }

        // ==========================================
        // CREAR PUBLICACIÓN
        // ==========================================
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreatePublication(CreatePublicationDto dto)
        {
            // 🔐 Obtener usuario desde JWT
            var idUser = User.FindFirst("IdUser")?.Value;

            if (idUser == null)
                return Unauthorized("Token inválido");

            // Validaciones
            if (string.IsNullOrWhiteSpace(dto.Titulo))
                return BadRequest("El título es obligatorio");

            if (string.IsNullOrWhiteSpace(dto.Descripcion))
                return BadRequest("La descripción es obligatoria");

            if (string.IsNullOrWhiteSpace(dto.Tipo_Contenido))
                return BadRequest("Tipo_Contenido es obligatorio");

            // Buscar usuario
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.IdUser == idUser);

            if (user == null)
                return BadRequest("Usuario no encontrado");

            // 🔥 Generar Id_Publicacion
            var publicationId =
                await _service.GenerateIdPublicacion();

            var publication = new Publication
            {
                Id_Publicacion = publicationId,
                Id_User = user.IdUser,

                // ✔ TU LÓGICA
                UserPosition = user.UserPosition,
                Tipo_Contenido = dto.Tipo_Contenido,

                Titulo = dto.Titulo,
                Descripcion = dto.Descripcion,
                CreatedAt = DateTime.UtcNow,

                // Inicialización importante
                Vistas = 0,
                Likes = 0,
                Dislikes = 0,
                TotalReportes = 0,
                Estado = "Activo"
            };

            _context.Publications.Add(publication);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Publicación creada correctamente",
                publication.Id_Publicacion
            });
        }

        // ==========================================
        // FEED (PUBLICACIONES)
        // ==========================================
        [HttpGet("feed")]
        public async Task<IActionResult> GetFeed()
        {
            var feed = await _context.Publications
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            return Ok(feed);
        }

        // ==========================================
        // OBTENER POR ID
        // ==========================================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var publication = await _context.Publications
                .FirstOrDefaultAsync(p => p.Id_Publicacion == id);

            if (publication == null)
                return NotFound("Publicación no encontrada");

            return Ok(publication);
        }

        // ==========================================
        // LIKES Y DISLIKES
        // ==========================================

        [HttpPost("like/{id}")]
        public async Task<IActionResult> Like(string id)
        {
            var pub = await _context.Publications.FirstOrDefaultAsync(p => p.Id_Publicacion == id);
            if (pub == null) return NotFound();

            pub.Likes++;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("dislike/{id}")]
        public async Task<IActionResult> Dislike(string id)
        {
            var pub = await _context.Publications
                .FirstOrDefaultAsync(p => p.Id_Publicacion == id);

            if (pub == null)
                return NotFound();

            // 👎 Incrementar dislike
            pub.Dislikes++;

            // 🚨 BLOQUEO AUTOMÁTICO
            if (pub.Dislikes >= 20 || pub.TotalReportes >= 5)
            {
                pub.Bloqueado_Por_Sistema = true;
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Dislike registrado",
                pub.Dislikes,
                pub.Bloqueado_Por_Sistema
            });
        }

        // ==========================================
        // ELIMINAR
        // ==========================================
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var publication = await _context.Publications
                .FirstOrDefaultAsync(p => p.Id_Publicacion == id);

            if (publication == null)
                return NotFound("Publicación no encontrada");

            _context.Publications.Remove(publication);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Publicación eliminada correctamente"
            });
        }
    }
}