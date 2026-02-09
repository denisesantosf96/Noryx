using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Noryx.API.Application.Dtos.Auth;
using Noryx.API.Application.Services;
using Noryx.API.Data;
using Noryx.API.Infrastructure.Security;

namespace Noryx.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly AppDbContext _context;
        private readonly ITokenService _tokenService;

        public AuthController(IAuthService authService, AppDbContext context, ITokenService tokenService)
        {
            _authService = authService;
            _context = context;
            _tokenService = tokenService;
        }


        [HttpPost("registrar")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Error = "Dados de registro incompletos ou inválidos." });

            try
            {
                await _authService.RegistrarAsync(request);
                return Ok(new { Message = "Usuário registrado com sucesso." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                // Chama o login
                var response = await _authService.LoginAsync(request);

                // Busca o usuário no banco
                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Email == request.Email);

                if (usuario == null)
                    return Unauthorized(new { Error = "Usuário não encontrado." });

                // Gera refresh token
                var refreshToken = _tokenService.GerarRefreshToken(usuario.Id);
                _context.RefreshTokens.Add(refreshToken);
                await _context.SaveChangesAsync();

                // Retorna JWT + refresh token
                return Ok(new
                {
                    Token = response.Token,
                    Expiracao = response.Expiracao,
                    RefreshToken = refreshToken.Token
                });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { Error = ex.Message });
            }
        }



        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Token))
                return BadRequest(new { Error = "Refresh token não informado." });

            var refreshToken = await _context.RefreshTokens
                .Include(rt => rt.Usuario)
                .FirstOrDefaultAsync(rt => rt.Token == request.Token && !rt.Revogado);

            if (refreshToken == null || refreshToken.ExpiraEm < DateTime.UtcNow)
                return Unauthorized(new { Error = "Refresh token inválido ou expirado." });

            var novoJwt = _tokenService.GerarToken(refreshToken.Usuario, out DateTime expiraEm);

            refreshToken.Revogado = true;
            var novoRefreshToken = _tokenService.GerarRefreshToken(refreshToken.UsuarioId);
            _context.RefreshTokens.Add(novoRefreshToken);

            await _context.SaveChangesAsync();

            return Ok(new RefreshTokenResponse
            {
                Token = novoJwt,
                Expiracao = expiraEm
            });
        }


        [Authorize]
        [HttpGet("protegido")]
        public IActionResult Protegido()
        {
            var usuarioId = User.FindFirst("sub")?.Value;
            var email = User.FindFirst("email")?.Value;
            var nome = User.FindFirst("nome")?.Value;

            return Ok(new
            {
                Mensagem = "Rota protegida acessada!",
                UsuarioId = usuarioId,
                Email = email,
                Nome = nome
            });
        }
    }
}
