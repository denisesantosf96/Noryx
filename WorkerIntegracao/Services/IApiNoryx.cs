using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Noryx.API.Application.Dtos;

namespace WorkerIntegracao.Services
{
    public interface IApiNoryx
    {
        Task<IEnumerable<MoedaExternaDto>> BuscarMoedasAsync();
        Task ImportarMoedasAsync(IEnumerable<MoedaExternaDto> moedas);
        Task InserirCotacaoAsync(CotacaoDto dto);
    }
}
