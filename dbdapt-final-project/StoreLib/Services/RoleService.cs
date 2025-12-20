using Microsoft.EntityFrameworkCore;
using StoreLib.Contexts;
using StoreLib.Models;

namespace StoreLib.Services
{
    public class RoleService(StoreDbContext context)
    {
        private readonly StoreDbContext _context = context;

        public Task<List<Role>> GetRolesAsync() 
            => _context.Roles.ToListAsync();

        public async Task<byte> GetRoleIdByNameAsync(string roleName)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName)
                ?? throw new ArgumentException("Роль не найдена");

            return role.RoleId;
        }
    }
}
