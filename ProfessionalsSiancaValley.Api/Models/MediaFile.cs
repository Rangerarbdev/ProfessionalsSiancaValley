using System.ComponentModel.DataAnnotations;

namespace ProfessionalsSiancaValley.Api.Models
{
    public class MediaFile
    {
        [Key]
        public int Id_Media { get; set; }

        public string Id_Publicacion { get; set; } = string.Empty;

        public string TipoArchivo { get; set; } = string.Empty;

        public string UrlArchivo { get; set; } = string.Empty;

        public string UrlMiniatura { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public string Tipo_Contenido { get; set; } = string.Empty;
    }
}
