namespace ProfessionalsSiancaValley.Api.Models
{
    public class PasswordRecovery
    {
        public int Id { get; set; }

        public string Email { get; set; } = string.Empty;

        public string RecoveryCode { get; set; } = string.Empty;

        public DateTime ExpiresAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
