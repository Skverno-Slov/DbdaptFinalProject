using StoreLib.Contexts;
using StoreLib.Services;

using StoreDbContext context = new();

AuthService authService = new(context);

while (true)
{
    Console.WriteLine("Исходный пароль:");
    var password = Console.ReadLine();

    var passwordHash = authService.HashPassword(password);

    if (authService.VerifyPassword(password, passwordHash))
        Console.WriteLine($"Хэш пароля(12, SHA512): {passwordHash}");
    else 
        Console.WriteLine("Ошибка хэширования");
}
