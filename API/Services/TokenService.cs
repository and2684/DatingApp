using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration config)        
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }

        public string CreateToken(AppUser user)
        {
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()), // Claims - что хранит токен (здесь субъект токена - это Id пользователя)
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username!), // Claims - что хранит токен (здесь субъект токена - это имя пользователя)
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature); // Ключ для токена

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7), // Время жизни токена
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();      // tokenHandler нужен для создания токена и его преобразования в string
            var token = tokenHandler.CreateToken(tokenDescriptor); // Создали токен
            return tokenHandler.WriteToken(token);                 // Вернули токен в виде string
        }
    }
}