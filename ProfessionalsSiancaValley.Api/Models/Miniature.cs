using System.ComponentModel.DataAnnotations;

namespace ProfessionalsSiancaValley.Api.Models
{
    public class Miniature
    {
        public int Id_Miniature { get; set; }

        [Required]
        public int Id_Publicacion { get; set; }

        [Required]
        public required string Id_User { get; set; }

        [Required]
        [MaxLength(150)]
        public required string Nombre_Usuario { get; set; }

        [Required]
        [MaxLength(150)]
        public required string Email_Usuario { get; set; }

        [Required]
        [MaxLength(10)]
        public required string Tipo_Contenido { get; set; }

        [Required]
        public required string Url_Miniatura { get; set; }

        [Required]
        [MaxLength(150)]
        public required string Titulo { get; set; }

        [Required] // 👈 AQUÍ obligas la descripción
        [MaxLength(250)]
        public required string Descripcion { get; set; }

        public DateTime Fecha_Publicacion { get; set; } = DateTime.UtcNow;

        public int Vistas { get; set; } = 0;
        public int Likes { get; set; } = 0;
        public int Dislikes { get; set; } = 0;

        public bool Es_Contenido_Sensible { get; set; } = false;
        public bool Bloqueado_Por_Sistema { get; set; } = false;

        public string Estado_Revision { get; set; } = "pendiente";
    }
}
