namespace ProfessionalsSiancaValley.Api.DTOs
{
    public class UserResponseDto
    {
        public string IdUser { get; set; } = string.Empty;

        public int UserPosition { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
    }
}

