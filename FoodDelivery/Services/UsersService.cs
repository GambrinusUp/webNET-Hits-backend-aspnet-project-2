using FoodDelivery.Models;
using FoodDelivery.Models.DTO;
using FoodDelivery.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FoodDelivery.Services
{
    public interface IUsersService
    {
        Task Register(UserRegisterDTO model);
        string? GetToken(LoginCredentials model);
        bool IsUserUnique(UserRegisterDTO model);
    }
    public class UsersService : IUsersService
    {
        private readonly Context _context;

        public UsersService(Context context)
        {
            _context = context;
        }

        public async Task Register(UserRegisterDTO model)
        {
            await _context.Users.AddAsync(ConverterDTO.Register(model));
            await _context.SaveChangesAsync();
        }

        public string? GetToken(LoginCredentials model)
        {
            var identity = GetIdentity(model.Email, model.Password);
            if (identity == null)
            {
                return null;
            }

            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                issuer: JwtConfigurations.Issuer,
                audience: JwtConfigurations.Audience,
                notBefore: now,
                claims: identity.Claims,
                expires: now.AddMinutes(JwtConfigurations.Lifetime),
                signingCredentials: new SigningCredentials(JwtConfigurations.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        private ClaimsIdentity GetIdentity(string email, string password)
        {

            var user = _context.Users.FirstOrDefault(x => x.Email == email && x.Password == password);
            if (user == null)
            {
                return null;
            }

            // Claims описывают набор базовых данных для авторизованного пользователя
            var claims = new List<Claim>{
            new Claim(ClaimTypes.Email, user.Email)
            };

            //Claims identity и будет являться полезной нагрузкой в JWT токене, которая будет проверяться стандартным атрибутом Authorize
            var claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }

        public bool IsUserUnique(UserRegisterDTO model)
        {
            if ((_context.Users.FirstOrDefault(x => x.FullName == model.FullName) == null) 
                && (_context.Users.FirstOrDefault(x => x.Email == model.Email) == null))
                return true;
            return false;
        }
    }
}
