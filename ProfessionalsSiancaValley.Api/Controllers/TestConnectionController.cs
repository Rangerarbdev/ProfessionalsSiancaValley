using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace ProfessionalsSiancaValley.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestConnectionController : ControllerBase
    {
        private readonly string _connectionString;

        public TestConnectionController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException(
                    "La cadena de conexión 'DefaultConnection' no está configurada en appsettings.json.");
        }

        [HttpGet]
        public IActionResult Test()
        {
            try
            {
                using var conn = new NpgsqlConnection(_connectionString);
                conn.Open();
                return Ok("Conexión exitosa a PostgreSQL ✅");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error de conexión: {ex.Message}");
            }
        }
    }
}


