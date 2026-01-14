using Noryx.API.Application.Dtos;

namespace Noryx.API.Application.Services
{
    public interface ICotacaoService
    {
        Task ProcessarCotacaoAsync(CotacaoDto dto);

        Task<CotacaoResponseDto?> ObterCotacaoAtualAsync(string moedaOrigem, string moedaDestino);

        Task<IEnumerable<HistoricoCotacaoDto>> ObterHistoricoAsync(string moedaOrigem, string moedaDestino);
    }
}
