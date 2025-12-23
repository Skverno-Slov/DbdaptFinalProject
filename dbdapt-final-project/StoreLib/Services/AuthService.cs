using Microsoft.IdentityModel.Tokens;
using StoreLib.Contexts;
using StoreLib.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StoreLib.Services
{
    /// <summary>
    /// Сервис для работы с jwt-токеном и хэшом паролей пользователей
    /// </summary>
    /// <param name="context">Контекст базы данных</param>
    public class AuthService(StoreDbContext context)
    {
        private readonly StoreDbContext _context = context;

        // Статическое поле с секретным ключом для jwt-токена
        public static string SecretKey { get => "ZFRSXHGTLDYBWJDTHIQNPHDSJDGHJFSU"; }


        /// <summary>
        /// метод для герерации jwt-токена
        /// </summary>
        /// <param name="user">авторизирующийся пользователь</param>
        /// <returns>JWT-токен</returns>
        public string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var roleName = _context.Roles.FirstOrDefault(r => r.RoleId == user.RoleId).Name;

            // включение в токен информации для передачи
            var claims = new Claim[]
            {
            new(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new(ClaimTypes.Name, user.Login),
            new(ClaimTypes.Role, roleName),
            };

            var token = new JwtSecurityToken(
                signingCredentials: credentials,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Проверка токена
        /// </summary>
        /// <param name="token">jwt-токен</param>
        /// <returns>true, если токен верный</returns>
        public bool IsValidToken(string token)
        {
            try
            {
                // Параметры проверки токена
                var tokenParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey)),
                    ValidateLifetime = true,
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(token, tokenParameters, out SecurityToken validatedToken);

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Метод для хэширования пароля
        /// </summary>
        /// <param name="password">Не хэшированый пароль</param>
        /// <returns>Хэш пароля (work factor: 12; hash type: SHA512)</returns>
        public string HashPassword(string password)
            => BCrypt.Net.BCrypt.EnhancedHashPassword(password, 12, BCrypt.Net.HashType.SHA512);

        /// <summary>
        /// Метод для проверки равенства паролей 
        /// </summary>
        /// <param name="password">Не хэштрованый пароль (из поля ввода)</param>
        /// <param name="hashedPassword">Хэшированый пароль (пользоваеля из БД)</param>
        /// <returns>true, если пароли совпадают</returns>
        public bool VerifyPassword(string password, string hashedPassword)
            => BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword, BCrypt.Net.HashType.SHA512);
    }
}
