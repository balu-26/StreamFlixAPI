using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StreamFlixAPI.Models;
using StreamFlixAPI.Data;
using StreamFlixAPI.DTOs;
using System.Security.Cryptography;
using System.Text;

namespace StreamFlixAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email || u.Username == dto.Username))
                return BadRequest(new { message = "User already exists." });

            using var hmac = new HMACSHA512();
            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password)),
                PasswordSalt = hmac.Key
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "User registered successfully." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
                return Unauthorized(new { message = "Invalid email or password" });

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password));

            if (!hash.SequenceEqual(user.PasswordHash))
                return Unauthorized(new { message = "Invalid email or password" });

            return Ok(new { message = "Login successful.", username = user.Username });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto dto)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
                if (user == null)
                    return NotFound(new { message = "User not found." });

                using var hmac = new HMACSHA512();
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.NewPassword));
                user.PasswordSalt = hmac.Key;

                await _context.SaveChangesAsync();
                return Ok(new { message = "Password updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Server error: " + ex.Message });
            }
        }
    }
}
