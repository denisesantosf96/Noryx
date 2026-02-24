using System.ComponentModel.DataAnnotations;

namespace Noryx.API.Domain.Entities
{
    public class Moeda : BaseEntity
    {
        [Required, StringLength(50)]
        public string Nome { get; set; } = null!;

        [Required, StringLength(3)]
        public string Codigo { get; set; } = null!; 

        public ICollection<Pais> Paises { get; set; } = new List<Pais>();
        public ICollection<Cotacao> Cotacoes { get; set; } = new List<Cotacao>();
    }
}
