namespace Noryx.API.Models
{
    public class Cotacao : BaseEntity
    {
        public int MoedaId { get; set; }
        public Moeda Moeda { get; set; } = null!;

        public decimal ValorCompra { get; set; }
        public decimal ValorVenda { get; set; }
        public decimal Variacao { get; set; }

        public DateTime DataReferencia { get; set; }

        public string Fonte { get; set; } = "AwesomeAPI";
    }
}
