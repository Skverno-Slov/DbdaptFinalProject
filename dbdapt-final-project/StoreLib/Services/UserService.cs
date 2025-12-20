using Microsoft.EntityFrameworkCore;
using StoreLib.Contexts;
using StoreLib.Models;

namespace StoreLib.Services
{
    public class UserService(StoreDbContext context)
    {
        private readonly StoreDbContext _context = context;

        public async Task<User> GetUserByLoginAsync(string login)
            => await _context.Users.FirstOrDefaultAsync(x => x.Login == login)
                ?? throw new ArgumentException($"Пользователь с логином {login} не найден");

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public bool IsUserExists(string login)
            => _context.Users.Any(u => u.Login == login);
    }
}
