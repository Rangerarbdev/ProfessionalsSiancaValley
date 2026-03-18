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

        public UploadMediaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file,
                                                string idPublicacion)
        {
            var folder = Path.Combine("wwwroot", "uploads");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);

            var path = Path.Combine(folder, fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var media = new MediaFile
            {
                Id_Publicacion = idPublicacion,
                TipoArchivo = file.ContentType.Contains("video") ? "video" : "image",
                UrlArchivo = "/uploads/" + fileName,
                CreatedAt = DateTime.UtcNow
            };

            _context.MediaFiles.Add(media);

            await _context.SaveChangesAsync();

            return Ok(media);
        }
    }
}
