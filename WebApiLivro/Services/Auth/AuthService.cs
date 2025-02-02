using WebApiLivro.Data;
using WebApiLivro.Dto.Auth;
using WebApiLivro.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System;

namespace WebApiLivro.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<IActionResult> Register(UserDto userDto)
        {
            if (await _context.Usuarios.AnyAsync(u => u.Email == userDto.Email))
                return new BadRequestObjectResult("Email já está em uso.");

            var usuario = new Usuario
            {
                Nome = userDto.Nome,
                Email = userDto.Email,
                SenhaHash = BCrypt.Net.BCrypt.HashPassword(userDto.Senha)
            };

            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();

            return new OkObjectResult("Usuário registrado com sucesso.");
        }

        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(loginDto.Senha, usuario.SenhaHash))
            {
                return new UnauthorizedObjectResult("Credenciais inválidas");
            }

            var token = GenerateJwtToken(usuario);

            return new OkObjectResult(new { Token = token });
        }

        public string GenerateJwtToken(Usuario usuario)
        {
            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public Task<IActionResult> Perfil()
        {
            throw new NotImplementedException();
        }
    }
}
