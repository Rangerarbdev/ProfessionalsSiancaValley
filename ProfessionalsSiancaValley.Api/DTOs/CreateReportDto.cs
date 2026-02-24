namespace ProfessionalsSiancaValley.Api.DTOs
{
    public class CreateReportDto
    {
        public int Id_Miniature { get; set; }

        public string Id_User { get; set; } = null!;
        public string Nombre_Usuario { get; set; } = null!;
        public string Email_Usuario { get; set; } = null!;
        public string Titulo { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
    }
}
