using Microsoft.IdentityModel.Tokens;
using StoreLib.Contexts;
using StoreLib.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace StoreLib.Services
{
    public class AuthService(StoreDbContext context)
    {
        private readonly StoreDbContext _context = context;

        private readonly string _secretKey = "ZFRSXHGTLDYBWJDTHIQNPHDSJDGHJFSU";

        public string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var roleName = _context.Roles.FirstOrDefault(r => r.RoleId == user.RoleId).Name;

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

        public bool IsValidToken(string token)
        {
            try
            {
                var tokenParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey)),
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

        public string HashPassword(string password)
            => BCrypt.Net.BCrypt.EnhancedHashPassword(password, 12, BCrypt.Net.HashType.SHA512);

        public bool VerifyPassword(string password, string hashedPassword)
            => BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword, BCrypt.Net.HashType.SHA512);
    }
}
