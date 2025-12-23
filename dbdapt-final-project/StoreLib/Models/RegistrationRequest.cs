namespace StoreLib.Models
{
    //Класс для запроса на регистрацию

    public class RegistrationRequest
    {
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;

        public string LastName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string? MiddleName { get; set; }
    }
}
