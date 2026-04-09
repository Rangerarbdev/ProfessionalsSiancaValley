using Microsoft.AspNetCore.Http;

namespace ProfessionalsSiancaValley.Api.DTOs
{
    public class UploadMediaDto
    {
        public string Id_Publicacion { get; set; } = string.Empty;
        public required IFormFile Archivo { get; set; }
    }
}
