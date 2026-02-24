using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Noryx.API.Application.Dtos;
using Noryx.API.Data;
using Noryx.API.Domain.Entities;

namespace Noryx.API.Controllers
{
    [ApiController]
    [Route("api/moeda")]
    public class MoedaController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public MoedaController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MoedaDto>>> Get()
        {
            var moedas = await _context.Moedas.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<MoedaDto>>(moedas));
        }

        [HttpPost("importar")]
        public async Task<IActionResult> ImportarMoedas(
            [FromBody] IEnumerable<MoedaExternaDto> moedas)
        {
            if (moedas == null || !moedas.Any())
                return BadRequest("Nenhuma moeda enviada.");

            var moedasDistintas = moedas
                .GroupBy(m => m.Codigo)
                .Select(g => g.First())
                .ToList();

            var codigosExistentes = await _context.Moedas
                .Select(m => m.Codigo)
                .ToListAsync();

            int adicionadas = 0;

            foreach (var moedaDto in moedasDistintas)
            {
                if (!codigosExistentes.Contains(moedaDto.Codigo))
                {
                    var moeda = new Moeda
                    {
                        Codigo = moedaDto.Codigo,
                        Nome = moedaDto.Nome
                    };

                    _context.Moedas.Add(moeda);
                    adicionadas++;
                }
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensagem = "Moedas sincronizadas com sucesso.",
                novas = adicionadas
            });
        }
    }
}
