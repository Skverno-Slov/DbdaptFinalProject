namespace StoreLib.Models
{
    // класс для запроса на авторизацию
    public class LoginRequest
    {
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
