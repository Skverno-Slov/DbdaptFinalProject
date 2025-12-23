using Microsoft.EntityFrameworkCore;
using StoreLib.Contexts;
using StoreLib.Models;

namespace StoreLib.Services
{
    /// <summary>
    /// Сервис для управления пользователями.
    /// </summary>
    public class UserService(StoreDbContext context)
    {
        private readonly StoreDbContext _context = context;

        /// <summary>
        /// Получает пользователя по логину
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <returns>Пользователь со связанными данными/</returns>
        public async Task<User?> GetUserByLoginAsync(string login)
            => await _context.Users
            .Include(u => u.Person)
            .Include(u => u.Role)
            .FirstOrDefaultAsync(x => x.Login == login);

        /// <summary>
        /// Добавляет нового пользователя
        /// </summary>
        /// <param name="user">Объект пользователя</param>
        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Получает пользователя по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Пользователь</returns>
        public async Task<User>? GetUserAsync(int id)
            => await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == id);

        /// <summary>
        /// Проверяет существование пользователя с указанным логином.
        /// </summary>
        /// <param name="login">Логин</param>
        /// <returns>true, если пользователь существует</returns>
        public bool IsUserExists(string login)
            => _context.Users.Any(u => u.Login == login);
    }
}
