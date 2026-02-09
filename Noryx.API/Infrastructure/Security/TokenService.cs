using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Noryx.API.Domain.Entities;

namespace Noryx.API.Infrastructure.Security
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config)
        {
            _config = config;
        }

        public string GerarToken(Usuario usuario, out DateTime expiraEm)
        {
            var secret = _config["Jwt:Secret"];
            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];
            var expiracaoMinutos = int.Parse(_config["Jwt:ExpiracaoMinutos"]);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            expiraEm = DateTime.UtcNow.AddMinutes(expiracaoMinutos);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
                new Claim("nome", usuario.Nome)
            };

            foreach (var usuarioRole in usuario.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, usuarioRole.Role));
            }


            var token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                expires: expiraEm,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public RefreshToken GerarRefreshToken(int usuarioId)
        {
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            return new RefreshToken
            {
                Token = token,
                UsuarioId = usuarioId,
                ExpiraEm = DateTime.UtcNow.AddDays(7), 
                Revogado = false
            };
        }

    }
}
