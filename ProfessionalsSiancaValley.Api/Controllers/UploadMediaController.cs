using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            if (archivo == null || archivo.Length == 0)
                return BadRequest("Archivo inválido");

            if (string.IsNullOrEmpty(Id_Publicacion))
                return BadRequest("Id_Publicacion requerido");

            // 📁 Carpeta uploads
            var uploadsPath = Path.Combine(_env.WebRootPath, "uploads");

            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            // 🔥 Nombre único
            var extension = Path.GetExtension(archivo.FileName).ToLower();
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

            // 🧠 Miniatura automática (básico por ahora)
            string urlMiniatura;

            if (tipoContenido == "image")
            {
                urlMiniatura = $"/uploads/{fileName}";
            }
            else
            {
                // 🔥 Video → usar imagen placeholder (luego mejoramos)
                urlMiniatura = "/uploads/video-default.png";
            }

            // 💾 Guardar en DB
            var media = new MediaFile
            {
                Id_Publicacion = Id_Publicacion,
                TipoArchivo = extension,
                Tipo_Contenido = tipoContenido,
                UrlArchivo = $"/uploads/{fileName}",
                UrlMiniatura = urlMiniatura,
                CreatedAt = DateTime.UtcNow
            };

            _context.MediaFiles.Add(media);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Archivo subido correctamente",
                media.UrlArchivo,
                media.UrlMiniatura,
                media.Tipo_Contenido
            });
        }
    }
}