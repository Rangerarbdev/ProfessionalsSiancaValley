using System.ComponentModel.DataAnnotations;

namespace ProfessionalsSiancaValley.Api.DTOs
{
    public class LoginDto
    {
        [Required]
        public string IdUser { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}

