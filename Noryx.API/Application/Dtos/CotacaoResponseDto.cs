namespace Noryx.API.Application.Dtos
{
    public class CotacaoResponseDto
    {
        public string MoedaOrigem { get; set; } = string.Empty;
        public string MoedaDestino { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public DateTime AtualizadoEm { get; set; }
    }
}
