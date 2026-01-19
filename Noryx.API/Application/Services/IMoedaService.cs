using Noryx.API.Application.Dtos;

namespace Noryx.API.Application.Services
{
    public interface IMoedaService
    {
        Task ImportarMoedasAsync(IEnumerable<MoedaExternaDto> moedasExternas);
    }
}
