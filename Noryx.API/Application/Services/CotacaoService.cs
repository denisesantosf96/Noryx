using Microsoft.EntityFrameworkCore;
using Noryx.API.Application.Dtos;
using Noryx.API.Data;
using Noryx.API.Domain.Entities;

namespace Noryx.API.Application.Services
{
    public class CotacaoService :ICotacaoService
    {
        private readonly AppDbContext _context;

        public CotacaoService(AppDbContext context)
        {
            _context = context;
        }
        public async Task ProcessarCotacaoAsync(CotacaoDto dto)
        {
            var moedaDestino = await _context.Moedas
                .FirstOrDefaultAsync(m => m.Codigo == dto.MoedaDestino);

            if (moedaDestino == null)
            {
                moedaDestino = new Moeda
                {
                    Codigo = dto.MoedaDestino,
                    Nome = dto.MoedaDestino
                };

                _context.Moedas.Add(moedaDestino);
                await _context.SaveChangesAsync();
            }

            var cotacaoAtual = await _context.Cotacoes
                .Where(c => c.MoedaId == moedaDestino.Id)
                .OrderByDescending(c => c.DataReferencia)
                .FirstOrDefaultAsync();

            if (cotacaoAtual != null && cotacaoAtual.ValorVenda == dto.Valor)
                return;

            var novaCotacao = new Cotacao
            {
                MoedaId = moedaDestino.Id,
                ValorVenda = dto.Valor,
                DataReferencia = DateTime.UtcNow,
                DataAtualizacao = DateTime.UtcNow
            };

            _context.Cotacoes.Add(novaCotacao);

            var historico = new HistoricoCotacao
            {
                MoedaOrigem = dto.MoedaOrigem,
                MoedaDestino = dto.MoedaDestino,
                Valor = dto.Valor,
                DataHora = DateTime.UtcNow
            };

            _context.HistoricosCotacoes.Add(historico);

            await _context.SaveChangesAsync();
        }

        public async Task<CotacaoResponseDto?> ObterCotacaoAtualAsync(
            string moedaOrigem,
            string moedaDestino)
        {
            var moeda = await _context.Moedas
                .FirstOrDefaultAsync(m => m.Codigo == moedaDestino);

            if (moeda == null)
                return null;

            var cotacao = await _context.Cotacoes
                .Where(c => c.MoedaId == moeda.Id)
                .OrderByDescending(c => c.DataReferencia)
                .FirstOrDefaultAsync();

            if (cotacao == null)
                return null;

            return new CotacaoResponseDto
            {
                MoedaOrigem = moedaOrigem,
                MoedaDestino = moedaDestino,
                Valor = cotacao.ValorVenda,
                AtualizadoEm = cotacao.DataReferencia
            };
        }

        public async Task<IEnumerable<HistoricoCotacaoDto>> ObterHistoricoAsync(
            string moedaOrigem,
            string moedaDestino)
        {
            return await _context.HistoricosCotacoes
                .Where(h =>
                    h.MoedaOrigem == moedaOrigem &&
                    h.MoedaDestino == moedaDestino)
                .OrderByDescending(h => h.DataHora)
                .Select(h => new HistoricoCotacaoDto
                {
                    MoedaOrigem = h.MoedaOrigem,
                    MoedaDestino = h.MoedaDestino,
                    Valor = h.Valor,
                    DataHora = h.DataHora
                })
                .ToListAsync();
        }

    }
}
