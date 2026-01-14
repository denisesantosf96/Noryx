using System.ComponentModel.DataAnnotations;

namespace Noryx.API.Models
{
    public class Categoria : BaseEntity
    {
        [Required, StringLength(50)]
        public string Nome { get; set; } = null!;

        public ICollection<Transacao> Transacoes { get; set; } = new List<Transacao>();
    }
}