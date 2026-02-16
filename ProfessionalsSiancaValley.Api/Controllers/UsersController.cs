using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ProfessionalsSiancaValley.Api.Data;
using ProfessionalsSiancaValley.Api.Models;
using ProfessionalsSiancaValley.Api.DTOs;

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
        public async Task<IActionResult> Create(UserCreateDto dto)
        {
            var hasher = new PasswordHasher<User>();

            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Dni = dto.Dni,
                ProfessionalLicense = dto.ProfessionalLicense,
                Specialty = dto.Specialty,
                University = dto.University,
                ProfessionalAssociationRegistration =
                    dto.ProfessionalAssociationRegistration,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email
            };

            // 🔐 HASH AUTOMÁTICO
            user.PasswordHash = hasher.HashPassword(user, dto.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var response = new UserResponseDto
            {
                IdUser = user.IdUser,
                UserPosition = user.UserPosition,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };

            return Ok(response);
        }
    }
}
