namespace Noryx.API.Application.Dtos
{
    public class HistoricoCotacaoDto
    {
        public string MoedaOrigem { get; set; } = string.Empty;
        public string MoedaDestino { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public DateTime DataHora { get; set; }
    }
}
