using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


namespace ProfessionalsSiancaValley.Api.Models
{
    [Table("users")]
    public class User
    {
        // ========================
        // PRIMARY KEY (GENERADO EN C#)
        // ========================
        [Key]
        [Column("id_user")]
        [MaxLength(20)]
        [JsonIgnore]
        public string IdUser { get; set; } = string.Empty;

        // ========================
        // POSICIÓN (SEQUENCE PostgreSQL)
        // ========================
        [Column("user_position")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        public int UserPosition { get; set; }

        // ========================
        // DATOS PERSONALES
        // ========================
        [Required]
        [Column("first_name")]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [Column("last_name")]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [Column("dni")]
        [MaxLength(20)]
        public string Dni { get; set; } = string.Empty;

        [Required]
        [Column("fecha_nacimiento")]
        public DateTime FechaNacimiento { get; set; }

        [Required]
        [Column("estado_edad")]
        public bool EstadoEdad { get; set; }


        [Column("professional_license")]
        [MaxLength(50)]
        public string? ProfessionalLicense { get; set; }

        [Column("specialty")]
        [MaxLength(100)]
        public string? Specialty { get; set; }

        [Column("university")]
        [MaxLength(150)]
        public string? University { get; set; }

        [Column("professional_association_registration")]
        [MaxLength(150)]
        public string? ProfessionalAssociationRegistration { get; set; }

        [Column("phone_number")]
        [MaxLength(30)]
        public string? PhoneNumber { get; set; }

        // ========================
        // LOGIN
        // ========================
        [Required]
        [Column("email")]
        [MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Column("password_hash")]
        public string PasswordHash { get; set; } = string.Empty;

        // ========================
        // CONTROL
        // ========================
        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Required]
        [Column("role")]
        [MaxLength(30)]
        public string Role { get; set; } = "User";

        [JsonIgnore]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonIgnore]
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}



