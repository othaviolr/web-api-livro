using WebApiLivro.Dto.Auth;
using Microsoft.AspNetCore.Mvc;

namespace WebApiLivro.Services.Auth
{
    public interface IAuthService
    {
        Task<IActionResult> Register(UserDto userDto); //validar com string depois, verificar a diferença!
        Task<IActionResult> Login(LoginDto loginDto);
        Task<IActionResult> Perfil();
    }
}
