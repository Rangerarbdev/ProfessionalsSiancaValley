namespace ProfessionalsSiancaValley.Api.DTOs
{
    public class CreateMiniatureDto
    {
        public int Id_Publicacion { get; set; }

        public string Id_User { get; set; } = null!;
        public string Nombre_Usuario { get; set; } = null!;
        public string Email_Usuario { get; set; } = null!;
        public string Tipo_Contenido { get; set; } = null!;
        public string Url_Miniatura { get; set; } = null!;
        public string Titulo { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
    }
}
