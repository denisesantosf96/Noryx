namespace Noryx.API.Infrastructure.Security
{
    public interface IPasswordHasherService
    {
        string Hash(string senha);
        bool Verificar(string senha, string senhaHash);
    }
}
