using Microsoft.AspNetCore.Mvc;
using StoreLib.Contexts;
using StoreLib.Models;
using StoreLib.Services;

namespace StoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController(StoreDbContext context) : ControllerBase
    {
        private const byte MinLoginLenth = 6;
        private const byte MinPasswordLenth = 6;

        private readonly AuthService _authService = new(context);
        private readonly RoleService _roleService = new(context);
        private readonly UserService _userService = new(context);
        private readonly PersonService _personService = new(context);

        [HttpPost("auth/register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequest request)
        {
            var password = request.Password;
            var login = request.Login;

            if (string.IsNullOrWhiteSpace(login))
                return BadRequest("Логинне может быть пустыми");

            if (login.Length < MinLoginLenth)
                return BadRequest($"Логин должен быть не менее {MinLoginLenth} символов");

            if (_userService.IsUserExists(login))
                return BadRequest("Пользователь с таким логином уже существует");

            if (string.IsNullOrWhiteSpace(password))
                return BadRequest("Пароль не может быть пустыми");

            if (password.Length < MinPasswordLenth)
                return BadRequest($"Пароль должен быть не менее {MinPasswordLenth} символов");

            if (string.IsNullOrEmpty(request.LastName))
                return BadRequest($"Введите фамилию");

            if (string.IsNullOrEmpty(request.FirstName))
                return BadRequest($"Введите имя");

            var person = await _personService.PostPersonByFullNameAsync(request.LastName,
                                                request.FirstName,
                                                request.MiddleName);

            var passwordHash = _authService.HashPassword(password);

            var user = new User
            {
                Login = login,
                HashPassword = passwordHash,
                RoleId = await _roleService.GetRoleIdByNameAsync("Клиент"),
                PersonId = person.PersonId
            };

            await _userService.AddUserAsync(user);

            return Ok();
        }

        [HttpPost("auth/login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            const string InvalidUserMessage = "Неверный логин или пароль";

            var login = request.Login;
            var password = request.Password;

            if (!_userService.IsUserExists(login))
                BadRequest(InvalidUserMessage);

            var user = await _userService.GetUserByLoginAsync(login);

            if (user == null)
                return BadRequest();

            if (!_authService.VerifyPassword(password, user.HashPassword))
                return Unauthorized(InvalidUserMessage);

            var token = _authService.GenerateToken(user);

            return Ok(new TokenResponse { Token = token });
        }
    }
}
