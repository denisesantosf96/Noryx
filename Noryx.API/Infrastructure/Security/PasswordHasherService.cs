using Microsoft.AspNetCore.Identity;

namespace Noryx.API.Infrastructure.Security
{
    public class PasswordHasherService : IPasswordHasherService
    {
        private readonly PasswordHasher<object> _hasher;

        public PasswordHasherService()
        {
            _hasher = new PasswordHasher<object>();
        }

        public string Hash(string senha)
        {
            return _hasher.HashPassword(null, senha);
        }

        public bool Verificar(string senha, string senhaHash)
        {
            var resultado = _hasher.VerifyHashedPassword(null, senhaHash, senha);
            return resultado == PasswordVerificationResult.Success;
        }
    }
}
