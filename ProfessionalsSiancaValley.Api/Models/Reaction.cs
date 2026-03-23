using System.ComponentModel.DataAnnotations;

namespace ProfessionalsSiancaValley.Api.Models
{
    public class Reaction
    {
        [Key]
        public int Id { get; set; }

        public string Id_Publicacion { get; set; } = string.Empty;

        public string Id_User { get; set; } = string.Empty;

        public string Tipo { get; set; } = string.Empty; // LIKE / DISLIKE

        public DateTime CreatedAt { get; set; }
    }
}
