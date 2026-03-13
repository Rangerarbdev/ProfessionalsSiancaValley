namespace ProfessionalsSiancaValley.Api.DTOs
{
    public class CreateMiniatureDto
    {
        public string? Id_Publicacion { get; set; }

        public string Tipo_Contenido { get; set; } = string.Empty;

        public string Url_Miniatura { get; set; } = string.Empty;

        public string Titulo { get; set; } = string.Empty;

        public string Descripcion { get; set; } = string.Empty;
    }
}
