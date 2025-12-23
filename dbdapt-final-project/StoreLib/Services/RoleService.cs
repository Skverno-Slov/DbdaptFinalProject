using Microsoft.EntityFrameworkCore;
using StoreLib.Contexts;
using StoreLib.Models;

namespace StoreLib.Services
{
    /// <summary>
    /// Сервис для управления ролями пользователей
    /// </summary>
    public class RoleService(StoreDbContext context)
    {
        private readonly StoreDbContext _context = context;

        /// <summary>
        /// Получает список всех ролей
        /// </summary>
        /// <returns>Список ролей</returns>
        public Task<List<Role>> GetRolesAsync()
            => _context.Roles.ToListAsync();

        /// <summary>
        /// Получает идентификатор роли по её названию
        /// </summary>
        /// <param name="roleName">Название роли</param>
        /// <returns>Идентификатор роли</returns>
        public async Task<byte> GetRoleIdByNameAsync(string roleName)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName)
                ?? throw new ArgumentException("Роль не найдена");

            return role.RoleId;
        }
    }
}
