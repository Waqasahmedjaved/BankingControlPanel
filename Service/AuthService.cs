using BankingControlPanel.DTOs;
using BankingControlPanel.Entities;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BankingControlPanel.Service
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;

        public AuthService(ApplicationDbContext context, IConfiguration configuration,
            ILogger<AuthService> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<string> RegisterAsync(RegistrationDTO model)
        {
            try
            {
                if (_context.Users.Any(u => u.Email == model.Email))
                    return "User already exists";

                var user = new User
                {
                    Email = model.Email,
                    Password = HashPassword(model.Password),
                    Role = model.Role
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return "User registered successfully";
            }
            catch (Exception ex)
            {
                _logger.LogError($"Method RegisterAsync Ex , {ex}");
            }
            return "Unable to process your request";
        }

        public async Task<string> LoginAsync(LogInDTO model)
        {
            try
            {
                var user = _context.Users.SingleOrDefault(u => u.Email == model.Email);
                if (user == null || !VerifyPassword(user.Password, model.Password))
                    throw new Exception("Invalid login");

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
                    }),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);

                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Method LoginAsync Ex , {ex}");
            }
            return null;
        }

        private bool VerifyPassword(string hashedPassword, string password)
        {
            return HashPassword(password) == hashedPassword;
        }
        private string HashPassword(string password)
        {
            byte[] salt = Encoding.UTF8.GetBytes(_configuration["PasswordSalt"]);
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
            return hashed;
        }
    }
}
