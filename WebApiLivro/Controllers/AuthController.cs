using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiLivro.Data;
using WebApiLivro.Dto.Auth;
using WebApiLivro.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace WebApiLivro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("Registrar")]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            try
            {
                // Verifica se o email já está em uso
                if (await _context.Usuarios.AnyAsync(u => u.Email == userDto.Email))
                    return BadRequest("Email já está em uso.");

                // Criação do usuário com os dados recebidos
                var usuario = new Usuario
                {
                    Nome = userDto.Nome,
                    Email = userDto.Email,
                    SenhaHash = BCrypt.Net.BCrypt.HashPassword(userDto.Senha)  // Agora, usa a Senha do DTO e a armazena como SenhaHash
                };

                // Adiciona o usuário ao banco de dados
                await _context.Usuarios.AddAsync(usuario);
                await _context.SaveChangesAsync();

                return Ok("Usuário registrado com sucesso.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
            if (usuario == null || !BCrypt.Net.BCrypt.Verify(loginDto.Senha, usuario.SenhaHash)) 
                return Unauthorized("Credenciais inválidas.");

            var token = GenerateJwtToken(usuario);
            return Ok(new { Token = token });
        }

        [Authorize]
        [HttpGet("Perfil")]
        public IActionResult Perfil()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Ok(new { Message = "Acesso autorizado.", UserId = userId });
        }

        private string GenerateJwtToken(Usuario usuario)
        {
            var claims = new[]
            {
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
    }
}
