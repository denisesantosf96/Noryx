using System.ComponentModel.DataAnnotations;

namespace Noryx.API.Models
{
    public class Pais : BaseEntity
    {
        [Required, StringLength(100)]
        public string Nome { get; set; } = null!;

        [StringLength(3)]
        public string? Sigla { get; set; }

        public ICollection<Moeda> Moedas { get; set; } = new List<Moeda>();
    }
}
