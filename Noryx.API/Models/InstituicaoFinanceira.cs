using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace Noryx.API.Models
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
