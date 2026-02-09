using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Noryx.API.Application.Dtos;
using Noryx.API.Application.Services;
using Noryx.API.Data;

namespace Noryx.API.Controllers
{
    [ApiController]
    [Route("api/moeda")]
    public class MoedaController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMoedaService _moedaService;

        public MoedaController(AppDbContext context, IMoedaService moedaService, IMapper mapper)
        {
            _context = context;
            _moedaService = moedaService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MoedaDto>>> Get()
        {
            var moedas = await _context.Moedas.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<MoedaDto>>(moedas));
        }

        [HttpPost]
        public async Task<IActionResult> Importar(
        [FromBody] IEnumerable<MoedaExternaDto> moedas)
        {
            await _moedaService.ImportarMoedasAsync(moedas);
            return Ok("Moedas importadas com sucesso");
        }
    }
}
