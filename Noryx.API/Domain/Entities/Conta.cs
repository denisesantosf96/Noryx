using System.ComponentModel.DataAnnotations;

namespace Noryx.API.Domain.Entities
{
    public class Conta : BaseEntity
    {
        [Required, StringLength(100)]
        public string Descricao { get; set; } = null!;

        public decimal Saldo { get; set; }

        
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;

        
        public int InstituicaoFinanceiraId { get; set; }
        public InstituicaoFinanceira InstituicaoFinanceira { get; set; } = null!;

        public ICollection<Transacao> Transacoes { get; set; } = new List<Transacao>();
    }
}
