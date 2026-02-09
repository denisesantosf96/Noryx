using Noryx.API.Application.Dtos.Auth;

namespace Noryx.API.Application.Services
{
    public interface IAuthService
    {
        Task RegistrarAsync(RegisterRequest request);
        Task<LoginResponse> LoginAsync(LoginRequest request);
    }
}
