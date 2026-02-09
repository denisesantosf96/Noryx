namespace Noryx.API.Domain.Entities
{
    public class Transacao : BaseEntity
    {
        public decimal Valor { get; set; }

        public DateTime Data { get; set; }

       
        public int ContaId { get; set; }
        public Conta Conta { get; set; } = null!;

        
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; } = null!;
    }
}
