namespace Noryx.API.Domain.Entities
{
    public class UsuarioRole
    {
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;

        public string Role { get; set; } = null!;

    }
}
