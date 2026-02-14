using Microsoft.AspNetCore.Mvc;
using ProfessionalsSiancaValley.Api.Data;
using ProfessionalsSiancaValley.Api.Models;

namespace ProfessionalsSiancaValley.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }
    }
}

