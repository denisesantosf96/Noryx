using Microsoft.EntityFrameworkCore;
using Noryx.API.Application.Dtos;
using Noryx.API.Data;
using Noryx.API.Domain.Entities;

namespace Noryx.API.Application.Services
{
    public class MoedaService : IMoedaService
    {
        private readonly AppDbContext _context;

        public MoedaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task ImportarMoedasAsync(IEnumerable<MoedaExternaDto> moedasExternas)
        {
            foreach (var moedaExterna in moedasExternas)
            {
                var existe = await _context.Moedas
                    .AnyAsync(m => m.Codigo == moedaExterna.Codigo);

                if (existe)
                    continue;

                _context.Moedas.Add(new Moeda
                {
                    Codigo = moedaExterna.Codigo,
                    Nome = moedaExterna.Nome
                });
            }

            await _context.SaveChangesAsync();
        }
    }
}
