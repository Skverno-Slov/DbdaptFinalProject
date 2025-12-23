using Microsoft.AspNetCore.Mvc;
using StoreLib.Contexts;
using StoreLib.Models;
using StoreLib.Services;

namespace StoreApi.Controllers
{
    /// <summary>
    /// Контроллер для управления регистрацией и авторизацией пользователей
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController(StoreDbContext context) : ControllerBase
    {
        private const byte MinLoginLenth = 6;
        private const byte MinPasswordLenth = 6;

        //создаие сервисов
        private readonly AuthService _authService = new(context);
        private readonly RoleService _roleService = new(context);
        private readonly UserService _userService = new(context);
        private readonly PersonService _personService = new(context);

        /// <summary>
        /// Регистрирует нового пользователя
        /// </summary>
        /// <param name="request">Данные для регистрации пользователя</param>
        /// <returns>200 OK при успешной регистрации</returns>
        /// <response code="200">Пользователь успешно зарегистрирован</response>
        /// <response code="400">
        /// Возможные ошибки:
        /// - Пустой логин или пароль
        /// - Логин менее 6 символов
        /// - Пользователь с таким логином уже существует
        /// - Пароль менее 6 символов
        /// - Не указана фамилия или имя
        /// </response>
        [HttpPost("auth/register")] //Эндпоинт
        public async Task<IActionResult> Register([FromBody] RegistrationRequest request) //[FromBody] - брать значение изтела запроса(Json-файла)
        {
            var password = request.Password;
            var login = request.Login;

            // валидация введёных данных
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

            //если человек существует в БД, использовать его, иначе создать нового
            var person = await _personService.PostPersonByFullNameAsync(request.LastName,
                                                request.FirstName,
                                                request.MiddleName);

            // Хэштрование пароля
            var passwordHash = _authService.HashPassword(password);

            var user = new User
            {
                Login = login,
                HashPassword = passwordHash,
                RoleId = await _roleService.GetRoleIdByNameAsync("Клиент"), //у всех новых пользователей роль - клиент
                PersonId = person.PersonId
            };

            await _userService.AddUserAsync(user);

            return Ok();
        }

        /// <summary>
        /// Аутентифицирует пользователя
        /// </summary>
        /// <param name="request">Данные для входа (логин и пароль)</param>
        /// <returns>
        /// JWT-токен для аутентифицированного пользователя.
        /// </returns>
        /// <response code="200">Возвращает JWT-токен</response>
        /// <response code="401">Неверный логин или пароль</response>
        [HttpPost("auth/login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            const string InvalidUserMessage = "Неверный логин или пароль";

            var login = request.Login;
            var password = request.Password;

            if (!_userService.IsUserExists(login))
                Unauthorized(InvalidUserMessage);

            var user = await _userService.GetUserByLoginAsync(login);

            if (user == null)
                return Unauthorized(InvalidUserMessage);

            if (!_authService.VerifyPassword(password, user.HashPassword))
                return Unauthorized(InvalidUserMessage);

            // после успешной авторизации генерировать и возвращать jwt-токен
            var token = _authService.GenerateToken(user);

            return Ok(new TokenResponse { Token = token });
        }
    }
}
