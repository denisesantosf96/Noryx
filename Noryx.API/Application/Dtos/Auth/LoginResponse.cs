namespace Noryx.API.Application.Dtos.Auth
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public DateTime Expiracao { get; set; }
    }
}
