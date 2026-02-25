using System.ComponentModel.DataAnnotations;

namespace ProfessionalsSiancaValley.Api.Models
{
    public class Report
    {
        [Key]
        public int Id_Report { get; set; }

        // Foreign Key
        public int Id_Miniature { get; set; }

        public string Id_User { get; set; } = null!;

        public string Nombre_Usuario { get; set; } = null!;

        public string Email_Usuario { get; set; } = null!;

        public string Titulo { get; set; } = null!;

        public string Descripcion { get; set; } = null!;

        public DateTime Fecha_Reporte { get; set; } = DateTime.UtcNow;

        // Navegación
        public Miniature Miniature { get; set; } = null!;
    }
}
