using Microsoft.AspNetCore.Mvc;
using Noryx.API.Application.Dtos;
using Noryx.API.Application.Services;

namespace Noryx.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CotacoesController : ControllerBase
    {
        private readonly ICotacaoService _cotacaoService;

        public CotacoesController(ICotacaoService cotacaoService)
        {
            _cotacaoService = cotacaoService;
        }

        /// <summary>
        /// Retorna a cotação atual de um par de moedas
        /// </summary>
        [HttpGet("{moedaOrigem}/{moedaDestino}")]
        public async Task<ActionResult<CotacaoResponseDto>> ObterCotacaoAtual(
            string moedaOrigem,
            string moedaDestino)
        {
            var resultado = await _cotacaoService
                .ObterCotacaoAtualAsync(moedaOrigem, moedaDestino);

            if (resultado == null)
                return NotFound("Cotação não encontrada.");

            return Ok(resultado);
        }

        /// <summary>
        /// Retorna o histórico de cotação de um par de moedas
        /// </summary>
        [HttpGet("{moedaOrigem}/{moedaDestino}/historico")]
        public async Task<ActionResult<IEnumerable<HistoricoCotacaoDto>>> ObterHistorico(
            string moedaOrigem,
            string moedaDestino)
        {
            var historico = await _cotacaoService
                .ObterHistoricoAsync(moedaOrigem, moedaDestino);

            return Ok(historico);
        }
    }
}
