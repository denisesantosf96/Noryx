using Microsoft.EntityFrameworkCore;
using Noryx.API.Application.Dtos.Auth;
using Noryx.API.Data;
using Noryx.API.Domain.Entities;
using Noryx.API.Infrastructure.Security;

namespace Noryx.API.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasherService _passwordHasher;
        private readonly ITokenService _tokenService;

        public AuthService(
            AppDbContext context,
            IPasswordHasherService passwordHasher,
            ITokenService tokenService)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
        }

        public async Task RegistrarAsync(RegisterRequest request)
        {
            try
            {
                var emailExiste = await _context.Usuarios
                    .AnyAsync(u => u.Email == request.Email);

                if (emailExiste)
                    throw new Exception("E-mail já cadastrado.");

                var senhaHash = _passwordHasher.Hash(request.Senha);

                var usuario = new Usuario(
                    request.Nome,
                    request.Email,
                    senhaHash
                );

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception($"Erro ao salvar usuário: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == request.Email && u.Ativo);

            if (usuario == null)
                throw new Exception("Usuário ou senha inválidos.");

            var senhaValida = _passwordHasher.Verificar(
                request.Senha,
                usuario.SenhaHash
            );

            if (!senhaValida)
                throw new Exception("Usuário ou senha inválidos.");

            var token = _tokenService.GerarToken(usuario, out DateTime expiraEm);

            return new LoginResponse
            {
                Token = token,
                Expiracao = expiraEm
            };
        }


    }
}
