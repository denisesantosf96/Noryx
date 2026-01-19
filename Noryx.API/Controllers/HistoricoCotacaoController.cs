using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Noryx.API.Application.Dtos;
using Noryx.API.Application.Services;
using Noryx.API.Data;

namespace Noryx.API.Controllers
{
    [ApiController]
    [Route("api/historico-cotacao")]
    public class HistoricoCotacaoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public HistoricoCotacaoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> Get(
            [FromQuery] string moedaOrigem,
            [FromQuery] string moedaDestino)
        {
            var historico = await _context.HistoricosCotacoes
                .Where(h => h.MoedaOrigem == moedaOrigem &&
                            h.MoedaDestino == moedaDestino)
                .OrderByDescending(h => h.DataHora)
                .ToListAsync();

            return Ok(historico);
        }
    }
}
