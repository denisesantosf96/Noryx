using System.ComponentModel.DataAnnotations;

namespace Noryx.API.Models
{
    public class Moeda : BaseEntity
    {
        [Required, StringLength(50)]
        public string Nome { get; set; } = null!;

        [Required, StringLength(3)]
        public string Codigo { get; set; } = null!; // BRL, USD, EUR

        public ICollection<Pais> Paises { get; set; } = new List<Pais>();
        public ICollection<Cotacao> Cotacoes { get; set; } = new List<Cotacao>();
    }
}
