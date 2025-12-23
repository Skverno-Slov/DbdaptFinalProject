using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StoreLib.Contexts;
using StoreLib.Models;
using StoreLib.Services;

namespace StoreWeb.Pages
{
    public class LoginModel(StoreDbContext context) : PageModel
    {
        private readonly UserService _userService = new(context);
        private readonly AuthService _authService = new(context);

        public void OnGet()
        {
            ErrorMessage = string.Empty;
        }

        [BindProperty]
        public User User { get; set; } = default!;

        [BindProperty]
        public string ErrorMessage { get; set; } = string.Empty;

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userService.GetUserByLoginAsync(User.Login);

            if (user is null || !_authService.VerifyPassword(User.HashPassword, user.HashPassword))
            {
                ErrorMessage = "Неверные логин или пароль.";
                return Page();
            }

            /*После успешного входа записывать в контекст полне имя (.SetString("FullName", $"{user.Person.LastName} {user.Person.FirstName} {user.Person.MiddleName}");)
             Id пользователя и роль, после перенаправлять на главную страницу*/
            HttpContext.Session
                .SetString("FullName", $"{user.Person.LastName} {user.Person.FirstName} {user.Person.MiddleName}");
            HttpContext.Session
                .SetString("UserId", $"{user.UserId}");
            HttpContext.Session
                .SetString("Role", user.Role.Name);
            return RedirectToPage("/Index");
        }

        public IActionResult OnGetLogout()
        {
            //Очистка даннных контекста и перенаправление на страницу входа
            HttpContext.Session.Clear();
            return RedirectToPage("/Login");
        }
    }
}
