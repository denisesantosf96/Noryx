using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerIntegracao.Models.Awesome;

namespace WorkerIntegracao.Services
{
    public interface IApiAwesome
    {
        Task<CotacaoAwesome> BuscarCotacaoAsync(string moedaOrigem, string moedaDestino);
    }
}
