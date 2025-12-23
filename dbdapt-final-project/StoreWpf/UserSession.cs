namespace StoreWpf
{
    public class UserSession
    {
        public static UserSession Instanse { get; set; } = new();

        public string Role { get; set; } = null!;
    }
}
