using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [HttpGet("tables")]
        public async Task<IActionResult> Tables()
        {
            using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new NpgsqlCommand(
                "SELECT table_name FROM information_schema.tables WHERE table_schema='public'",
                conn);

            var reader = await cmd.ExecuteReaderAsync();

            var tables = new List<string>();

            while (await reader.ReadAsync())
            {
                tables.Add(reader.GetString(0));
            }

            return Ok(tables);
        }
    }
}


