using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace Noryx.API.Models
{
    public class Usuario : BaseEntity
    {
        [Required, StringLength(100)]
        public string Nome { get; set; } = null!;

        [Required, StringLength(150)]
        public string Email { get; set; } = null!;

        [Required]
        public string SenhaHash { get; set; } = null!;

        public bool Ativo { get; set; } = true;

        public ICollection<Conta> Contas { get; set; } = new List<Conta>();
    }
}
