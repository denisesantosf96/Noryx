using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Noryx.API.Application.Dtos;
using Noryx.API.Data;

namespace Noryx.API.Controllers
{
    [ApiController]
    [Route("api/referencias")]
    public class ReferenciasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReferenciasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("moedas")]
        public async Task<ActionResult<IEnumerable<MoedaDto>>> ObterMoedas()
        {
            var moedas = await _context.Moedas
                .Select(m => new MoedaDto
                {
                    Id = m.Id,
                    Nome = m.Nome,
                    Codigo = m.Codigo
                })
                .ToListAsync();

            return Ok(moedas);
        }

        [HttpGet("paises")]
        public async Task<ActionResult<IEnumerable<PaisDto>>> ObterPaises()
        {
            var paises = await _context.Paises
                .Select(p => new PaisDto
                {
                    Id = p.Id,
                    Nome = p.Nome
                })
                .ToListAsync();

            return Ok(paises);
        }

        [HttpGet("categorias")]
        public async Task<ActionResult<IEnumerable<CategoriaDto>>> ObterCategorias()
        {
            var categorias = await _context.Categorias
                .OrderBy(c => c.Nome)
                .Select(c => new CategoriaDto
                {
                    Id = c.Id,
                    Nome = c.Nome
                })
                .ToListAsync();

            return Ok(categorias);
        }
    }
}
