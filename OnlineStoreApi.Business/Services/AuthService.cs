using OnlineStoreApi.Business.DTOs;
using OnlineStoreApi.Data;
using OnlineStoreApi.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace OnlineStoreApi.Business.Services
{
    public interface IAuthService
    {
        Task<TokenResponseDto> RegisterAsync(RegisterDto dto);
        Task<TokenResponseDto> LoginAsync(LoginDto dto);
        UserDto MapToDto(User user);
    }

    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<TokenResponseDto> RegisterAsync(RegisterDto dto)
        {
            // Check if user already exists
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (existingUser != null)
                throw new InvalidOperationException("User with this email already exists");

            // Create new user with hashed password
            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var token = GenerateJwtToken(user);
            return new TokenResponseDto
            {
                UserId = user.Id,
                Email = user.Email,
                Name = user.Name,
                AccessToken = token,
                RefreshToken = GenerateRefreshToken(),
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            };
        }

        public async Task<TokenResponseDto> LoginAsync(LoginDto dto)
        {
            // Find user by email
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null)
                throw new InvalidOperationException("Invalid email or password");

            // Verify password
            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new InvalidOperationException("Invalid email or password");

            var token = GenerateJwtToken(user);
            return new TokenResponseDto
            {
                UserId = user.Id,
                Email = user.Email,
                Name = user.Name,
                AccessToken = token,
                RefreshToken = GenerateRefreshToken(),
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            };
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim("UserId", user.Id.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public UserDto MapToDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                CreatedAt = user.CreatedAt
            };
        }
    }
}
