using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Noryx.API.Application.Dtos;
using Noryx.API.Data;
using Noryx.API.Models;

namespace Noryx.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDto>>> Listar()
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

        [HttpPost]
        public async Task<ActionResult> Criar(CategoriaCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nome))
                return BadRequest("Nome é obrigatório.");

            var existe = await _context.Categorias
                .AnyAsync(c => c.Nome == dto.Nome);

            if (existe)
                return Conflict("Categoria já existe.");

            var categoria = new Categoria
            {
                Nome = dto.Nome
            };

            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Listar), new { id = categoria.Id }, null);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Remover(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);

            if (categoria == null)
                return NotFound();

            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
