using System.ComponentModel.DataAnnotations;

namespace ProfessionalsSiancaValley.Api.Models
{
    public class Publication
    {
        [Key]
        public string Id_Publicacion { get; set; } = string.Empty;

        public string Id_User { get; set; } = string.Empty;

        public int UserPosition { get; set; }

        public string Tipo_Contenido { get; set; } = string.Empty;

        public string Titulo { get; set; } = string.Empty;

        public string Descripcion { get; set; } = string.Empty;

        public string Nombre_Usuario { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public string Estado { get; set; } = "ACTIVO";

        public int Vistas { get; set; } = 0;

        public int Likes { get; set; } = 0;

        public int Dislikes { get; set; } = 0;

        public int TotalReportes { get; set; } = 0;

        public bool Bloqueado_Por_Sistema { get; set; } = false;
    }
}
