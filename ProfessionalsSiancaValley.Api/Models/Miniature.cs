using System.ComponentModel.DataAnnotations;

namespace ProfessionalsSiancaValley.Api.Models
{
    public class Miniature
    {
        [Key]
        public int Id_Miniature { get; set; }

        public int Id_Publicacion { get; set; }

        public string Id_User { get; set; } = null!;

        public string Nombre_Usuario { get; set; } = null!;

        public string Email_Usuario { get; set; } = null!;

        public string Tipo_Contenido { get; set; } = null!;

        public string Url_Miniatura { get; set; } = null!;

        public string Titulo { get; set; } = null!;

        public string Descripcion { get; set; } = null!;

        public DateTime Fecha_Publicacion { get; set; } = DateTime.UtcNow;

        public int Vistas { get; set; } = 0;
        public int Likes { get; set; } = 0;
        public int Dislikes { get; set; } = 0;

        public bool Es_Contenido_Sensible { get; set; } = false;
        public bool Bloqueado_Por_Sistema { get; set; } = false;

        public string Estado_Revision { get; set; } = "pendiente";

        public int TotalReportes { get; set; }
    }
}
