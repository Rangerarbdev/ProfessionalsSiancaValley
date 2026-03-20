using Microsoft.AspNetCore.Http;

namespace ProfessionalsSiancaValley.Api.DTOs
{
    public class UploadMediaDto
    {
        public string Id_Publicacion { get; set; }
        public IFormFile Archivo { get; set; }
    }
}
