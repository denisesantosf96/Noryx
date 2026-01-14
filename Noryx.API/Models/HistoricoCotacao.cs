namespace Noryx.API.Models
{
    public class HistoricoCotacao : BaseEntity
    {
        public string MoedaOrigem { get; set; } = string.Empty;
        public string MoedaDestino { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public DateTime DataHora { get; set; }
    }
}