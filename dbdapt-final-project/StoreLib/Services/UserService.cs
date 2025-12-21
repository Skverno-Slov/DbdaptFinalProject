using Microsoft.EntityFrameworkCore;
using StoreLib.Contexts;
using StoreLib.Models;

namespace StoreLib.Services
{
    public class UserService(StoreDbContext context)
    {
        private readonly StoreDbContext _context = context;

        public async Task<User?> GetUserByLoginAsync(string login)
            => await _context.Users
            .Include(u => u.Person)
            .Include(u => u.Role)
            .FirstOrDefaultAsync(x => x.Login == login);

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User>? GetUserAsync(int id) 
            => await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == id);

        public bool IsUserExists(string login)
            => _context.Users.Any(u => u.Login == login);
    }
}
