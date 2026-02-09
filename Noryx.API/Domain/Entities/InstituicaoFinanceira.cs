using System.ComponentModel.DataAnnotations;

namespace Noryx.API.Domain.Entities
{
    public class InstituicaoFinanceira : BaseEntity
    {
        [Required, StringLength(100)]
        public string Nome { get; set; } = null!;

        [StringLength(20)]
        public string? Codigo { get; set; }

        public ICollection<Conta> Contas { get; set; } = new List<Conta>();
    }
}
