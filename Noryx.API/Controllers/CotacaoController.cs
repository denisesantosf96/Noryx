using Microsoft.AspNetCore.Mvc;
using Noryx.API.Application.Dtos;
using Noryx.API.Application.Services;

namespace Noryx.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CotacaoController : ControllerBase
    {
        private readonly ICotacaoService _cotacaoService;

        public CotacaoController(ICotacaoService cotacaoService)
        {
            _cotacaoService = cotacaoService;
        }

        [HttpGet("{moedaOrigem}/{moedaDestino}")]
        public async Task<ActionResult<CotacaoResponseDto>> ObterCotacaoAtual(
            string moedaOrigem,
            string moedaDestino)
        {
            var resultado = await _cotacaoService
                .ObterCotacaoAtualAsync(moedaOrigem, moedaDestino);

            if (resultado == null)
                return NotFound();

            return Ok(resultado);
        }

        [HttpGet("{moedaOrigem}/{moedaDestino}/historico")]
        public async Task<ActionResult<IEnumerable<HistoricoCotacaoDto>>> ObterHistorico(
            string moedaOrigem,
            string moedaDestino)
        {
            var historico = await _cotacaoService
                .ObterHistoricoAsync(moedaOrigem, moedaDestino);

            return Ok(historico);
        }

        [HttpPost]
        public async Task<IActionResult> ProcessarCotacao([FromBody] CotacaoDto dto)
        {
            await _cotacaoService.ProcessarCotacaoAsync(dto);
            return Ok();
        }
    }
}
