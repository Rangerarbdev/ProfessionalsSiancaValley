using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        private readonly PasswordHasher<User> _hasher;

        public UsersController(AppDbContext context)
        {
            _context = context;
            _hasher = new PasswordHasher<User>();
        }

        // =============================
        // REGISTER
        // POST api/users/register
        // =============================
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserCreateDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
                return BadRequest("El email ya está registrado");

            var edad = DateTime.Today.Year - dto.FechaNacimiento.Year;
            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Dni = dto.Dni,
                FechaNacimiento = dto.FechaNacimiento,
                EstadoEdad = edad >= 18,
                ProfessionalLicense = dto.ProfessionalLicense,
                Specialty = dto.Specialty,
                University = dto.University,
                ProfessionalAssociationRegistration = dto.ProfessionalAssociationRegistration,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email
            };

            user.PasswordHash = _hasher.HashPassword(user, dto.Password);

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

        // =============================
        // LOGIN
        // POST api/users/login
        // =============================
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null)
                return Unauthorized("Usuario no encontrado");

            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);

            if (result == PasswordVerificationResult.Failed)
                return Unauthorized("Contraseña incorrecta");

            return Ok(new
            {
                message = "Login correcto",
                user.IdUser,
                user.FirstName,
                user.LastName,
                user.Email,
                user.Role
            });
        }

        // =============================
        // FORGOT PASSWORD
        // POST api/users/forgot-password
        // =============================
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null)
                return BadRequest("Email no registrado");

            var code = new Random().Next(100000, 999999).ToString();

            var recovery = new PasswordRecovery
            {
                Email = dto.Email,
                RecoveryCode = code,
                ExpiresAt = DateTime.UtcNow.AddMinutes(10)
            };

            _context.PasswordRecovery.Add(recovery);
            await _context.SaveChangesAsync();

            // aquí luego se enviará email
            return Ok(new
            {
                message = "Código generado",
                code = code
            });
        }

        // =============================
        // RESET PASSWORD
        // POST api/users/reset-password
        // =============================
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
        {
            var recovery = await _context.PasswordRecovery
                .FirstOrDefaultAsync(r =>
                    r.Email == dto.Email &&
                    r.RecoveryCode == dto.Code &&
                    r.ExpiresAt > DateTime.UtcNow);

            if (recovery == null)
                return BadRequest("Código inválido o expirado");

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null)
                return BadRequest("Usuario no encontrado");

            user.PasswordHash = _hasher.HashPassword(user, dto.NewPassword);

            _context.PasswordRecovery.Remove(recovery);

            await _context.SaveChangesAsync();

            return Ok("Contraseña actualizada correctamente");
        }
    }
}