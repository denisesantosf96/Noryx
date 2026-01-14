namespace Noryx.API.Models
{
    public class Transacao : BaseEntity
    {
        public decimal Valor { get; set; }

        public DateTime Data { get; set; }

        // Conta
        public int ContaId { get; set; }
        public Conta Conta { get; set; } = null!;

        // Categoria
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; } = null!;
    }
}
