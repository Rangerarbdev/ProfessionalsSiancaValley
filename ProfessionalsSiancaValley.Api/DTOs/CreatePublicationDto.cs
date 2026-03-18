namespace ProfessionalsSiancaValley.Api.DTOs
{
    public class CreatePublicationDto
    {
        public string Id_User { get; set; } = string.Empty;

        public string Tipo_Contenido { get; set; } = string.Empty;

        public string Titulo { get; set; } = string.Empty;

        public string Descripcion { get; set; } = string.Empty;
    }
}
