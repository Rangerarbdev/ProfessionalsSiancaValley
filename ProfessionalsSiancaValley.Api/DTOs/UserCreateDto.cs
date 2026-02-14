using System.ComponentModel.DataAnnotations;

namespace ProfessionalsSiancaValley.Api.DTOs
{
    public class UserCreateDto
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public string Dni { get; set; } = string.Empty;

        public string? ProfessionalLicense { get; set; }

        public string? Specialty { get; set; } = string.Empty;

        public string? University { get; set; } = string.Empty;

        public string? ProfessionalAssociationRegistration { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}

