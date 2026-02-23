using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProfessionalsSiancaValley.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PublicacionesController : ControllerBase
    {
        [Authorize(Policy = "MayorDeEdad")]
        [HttpPost("crear")]
        public IActionResult CrearPublicacion()
        {
            return Ok("Publicación creada");
        }
    }
}

