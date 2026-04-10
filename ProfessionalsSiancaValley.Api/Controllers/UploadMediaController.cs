using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProfessionalsSiancaValley.Api.Data;
using ProfessionalsSiancaValley.Api.Models;

namespace ProfessionalsSiancaValley.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadMediaController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public UploadMediaController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Upload(
            [FromForm] string Id_Publicacion,
            [FromForm] IFormFile archivo)
        {
            // ✅ Validar archivo
            if (archivo == null || archivo.Length == 0)
                return BadRequest("Archivo inválido");

            // ✅ Validar ID publicación
            if (string.IsNullOrEmpty(Id_Publicacion))
                return BadRequest("Id_Publicacion requerido");

            // ✅ Validar extensión
            var extension = Path.GetExtension(archivo.FileName).ToLower();

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".mp4", ".webm", ".ogg" };

            if (!allowedExtensions.Contains(extension))
                return BadRequest("Tipo de archivo no permitido");

            // ✅ Validar tamaño (máx 10MB)
            var maxSize = 10 * 1024 * 1024;
            if (archivo.Length > maxSize)
                return BadRequest("El archivo supera el tamaño permitido (10MB)");

            // ✅ Validar que exista la publicación
            var exists = await _context.Publications
                .AnyAsync(p => p.Id_Publicacion == Id_Publicacion);

            if (!exists)
                return NotFound("La publicación no existe");

            // 📁 Carpeta uploads
            var uploadsPath = Path.Combine(_env.WebRootPath, "uploads");

            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            // 🔥 Nombre único
            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadsPath, fileName);

            // 💾 Guardar archivo
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await archivo.CopyToAsync(stream);
            }

            // 🧠 Tipo contenido
            string tipoContenido = extension switch
            {
                ".jpg" or ".jpeg" or ".png" => "image",
                ".mp4" or ".webm" or ".ogg" => "video",
                _ => "unknown"
            };

            // 🌐 Base URL (para frontend)
            var baseUrl = $"{Request.Scheme}://{Request.Host}";

            var urlArchivo = $"{baseUrl}/uploads/{fileName}";

            // 🧠 Miniatura
            string urlMiniatura = tipoContenido == "image"
                ? urlArchivo
                : $"{baseUrl}/uploads/video-default.png";

            // 💾 Guardar en DB
            var media = new MediaFile
            {
                Id_Publicacion = Id_Publicacion,
                TipoArchivo = extension,
                Tipo_Contenido = tipoContenido,
                UrlArchivo = urlArchivo,
                UrlMiniatura = urlMiniatura,
                CreatedAt = DateTime.UtcNow
            };

            _context.MediaFiles.Add(media);
            await _context.SaveChangesAsync();

            // ✅ Respuesta limpia
            return Ok(new
            {
                message = "Archivo subido correctamente",
                urlArchivo,
                urlMiniatura,
                tipoContenido
            });
        }
    }
}