namespace Noryx.API.Application.Dtos.Auth
{
    public class RefreshTokenResponse
    {
        public string Token { get; set; } = null!;
        public DateTime Expiracao { get; set; }
    }
}
