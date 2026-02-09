using Noryx.API.Domain.Entities;

namespace Noryx.API.Infrastructure.Security
{
    public interface ITokenService
    {
        string GerarToken(Domain.Entities.Usuario usuario, out DateTime expiraEm);
        RefreshToken GerarRefreshToken(int usuarioId);
    }
}
