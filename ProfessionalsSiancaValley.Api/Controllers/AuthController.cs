using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProfessionalsSiancaValley.Api.Data;
using ProfessionalsSiancaValley.Api.DTOs;
using ProfessionalsSiancaValley.Api.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;



namespace ProfessionalsSiancaValley.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDto dto)
        {
            var user = _context.Users
    .       FirstOrDefault(u => u.Email == dto.Email);

            if (user == null)
                return Unauthorized("Usuario no encontrado");

            var hasher = new PasswordHasher<User>();

            var result = hasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                dto.Password);

            if (result == PasswordVerificationResult.Failed)
                return Unauthorized("Credenciales inválidas");

            var token = GenerarToken(user);

            return Ok(new { token });
        }

        [Authorize]
        [HttpGet("session")]
        public IActionResult Session()
        {
            var idUser = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var edad = User.FindFirst("edad")?.Value;
            var estadoEdad = User.FindFirst("estadoEdad")?.Value;

            if (idUser == null)
                return Unauthorized();

            return Ok(new
            {
                authenticated = true,
                idUser,
                email,
                edad = int.Parse(edad!),
                estadoEdad = bool.Parse(estadoEdad!)
            });
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(UserCreateDto model)
        {
            // PASO A — Validar fecha futura
            if (model.FechaNacimiento > DateTime.Today)
            {
                return BadRequest("Fecha inválida.");
            }

            // PASO B — Calcular edad
            int edad = CalcularEdad(model.FechaNacimiento);

            if (edad < 18)
            {
                return BadRequest("Debes ser mayor de 18 años para registrarte.");
            }

            // Verificar si el email ya existe
            if (_context.Users.Any(u => u.Email == model.Email))
            {
                return BadRequest("El email ya está registrado.");
            }

            var hasher = new PasswordHasher<User>();

            var user = new User
            {
                IdUser = Guid.NewGuid().ToString("N").Substring(0, 20),

                FirstName = model.FirstName,
                LastName = model.LastName,
                Dni = model.Dni,

                FechaNacimiento = DateTime.SpecifyKind(model.FechaNacimiento, DateTimeKind.Utc),
                EstadoEdad = true,

                ProfessionalLicense = model.ProfessionalLicense,
                Specialty = model.Specialty,
                University = model.University,
                ProfessionalAssociationRegistration = model.ProfessionalAssociationRegistration,
                PhoneNumber = model.PhoneNumber,

                Email = model.Email.ToLower(),
                Role = "User",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            user.PasswordHash = hasher.HashPassword(user, model.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("Usuario registrado correctamente");
        }

        private int CalcularEdad(DateTime fechaNacimiento)
        {
            var hoy = DateTime.Today;
            var edad = hoy.Year - fechaNacimiento.Year;

            if (fechaNacimiento.Date > hoy.AddYears(-edad))
                edad--;

            return edad;
        }


        private string GenerarToken(User user)
        {
            var jwt = _config.GetSection("Jwt");

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwt["Key"]!));

            var creds = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            int edad = CalcularEdad(user.FechaNacimiento);

            var claims = new[]
            {
        new Claim(ClaimTypes.NameIdentifier, user.IdUser),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Role, user.Role), // 👈 ESTA ES LA CLAVE
        new Claim("edad", edad.ToString()),
        new Claim("estadoEdad", user.EstadoEdad.ToString())
    };

            var token = new JwtSecurityToken(
                issuer: jwt["Issuer"],
                audience: jwt["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    int.Parse(jwt["ExpireMinutes"]!)),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }
    }
}

