namespace Noryx.API.Models
{
    public class LoginResponse
    {
        public string Token { get; set; } = null!;
        public DateTime ExpiraEm { get; set; }
        public string Nome { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
