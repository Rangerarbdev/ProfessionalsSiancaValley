using System.ComponentModel.DataAnnotations;

namespace ProfessionalsSiancaValley.Api.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Publication
    {
        [Key]
        public string Id_Publicacion { get; set; }

        public string Id_User { get; set; }

        public int UserPosition { get; set; }

        public string Tipo_Contenido { get; set; }

        public string Titulo { get; set; }

        public string Descripcion { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Estado { get; set; } = "ACTIVO";

        public int Vistas { get; set; } = 0;

        public int Likes { get; set; } = 0;

        public int Dislikes { get; set; } = 0;

        public int TotalReportes { get; set; } = 0;
    }
}
