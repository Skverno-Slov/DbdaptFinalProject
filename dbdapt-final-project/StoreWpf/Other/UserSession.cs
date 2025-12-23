namespace StoreWpf.Other
{
    public class UserSession // Класс для реализации сеансов пользователей (паттерн: Singleton )
    {
        private static UserSession _instance;

        private UserSession() {}

        public static UserSession Instance
        {
            get
            {
                _instance ??= new UserSession();
                return _instance; //если _instance - null, инициализировать
            }
        } //статическое свойство для создания единстванного экземпляра класса

        public string Role { get; set; } = null!; //Тут роль
    }
}
