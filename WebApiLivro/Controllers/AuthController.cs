using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApiLivro.Dto.Auth;
using WebApiLivro.Models;
using WebApiLivro.Services.Auth;
using WebApiLivro.Data;

namespace WebApiLivro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly AppDbContext _context;

        public AuthController(IAuthService authService, AppDbContext context)
        {
            _authService = authService;
            _context = context;
        }

        [HttpPost("Registrar")]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            try
            {
                if (await _context.Usuarios.AnyAsync(u => u.Email == userDto.Email))
                {
                    return BadRequest("Email já está em uso.");
                }

                var usuario = new Usuario
                {
                    Nome = userDto.Nome,
                    Email = userDto.Email,
                    SenhaHash = BCrypt.Net.BCrypt.HashPassword(userDto.Senha) 
                };

                await _context.Usuarios.AddAsync(usuario);
                await _context.SaveChangesAsync();

                return Ok("Usuário registrado com sucesso.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao registrar usuário: {ex.Message}");
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            try
            {
                var token = await _authService.Login(loginDto);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return Unauthorized($"Erro ao fazer login: {ex.Message}");
            }
        }

        [Authorize]
        [HttpGet("Perfil")]
        public IActionResult Perfil()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Ok(new { Message = "Acesso autorizado.", UserId = userId });
        }
    }
}
