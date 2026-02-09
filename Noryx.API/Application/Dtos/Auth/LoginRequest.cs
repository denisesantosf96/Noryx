using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Noryx.API.Application.Dtos.Auth
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string Senha { get; set; } = null!;
    }
}
