using System.ComponentModel.DataAnnotations;

namespace Noryx.API.Application.Dtos.Auth
{
    public class RegisterRequest
    {
        [Required]
        [MaxLength(100)]
        public string Nome { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(150)]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Senha { get; set; }
    }
}
