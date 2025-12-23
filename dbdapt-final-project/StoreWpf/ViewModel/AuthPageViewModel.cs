using StoreLib.Contexts;
using StoreLib.Services;
using StoreWpf.Commands;
using StoreWpf.Other;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace StoreWpf.ViewModel
{
    //ViewModel для страницы авторизации 
    public class AuthPageViewModel : INotifyPropertyChanged
    {
        private static readonly StoreDbContext _context = App.StoreDbContext; //получение контекста из статического свойства App

        private readonly AuthService _authService = new(_context);
        private readonly UserService _userService = new(_context);

        //поля
        private string? _login;
        private string? _password;
        private bool _isPasswordVisible;

        //Команды
        private RelayCommand _command;

        public RelayCommand Command
        {
            get => _command ??= new RelayCommand(async obj => await VerifyUser(),
                obj => CheckUserInput()); // Команда может выолнится, если CheckUserInput - это true, VerifyUser - логика команжы
        }

        //свойства для привязки
        public string? Login
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged(); //Уведомление об изменении
            }
        }
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public bool IsPasswordVisible
        {
            get => _isPasswordVisible;
            set
            {
                _isPasswordVisible = value;
                OnPropertyChanged();
            }
        }

        private async Task VerifyUser()
        {
            try 
            { 
                var user = await _userService.GetUserByLoginAsync(Login);

                if (user is null)
                {
                    ShowError("Не верный логин или пароль", "Ошибка");
                    return;
                }

                if (!_authService.VerifyPassword(Password, user.HashPassword))
                {
                    ShowError("Не верный логин или пароль", "Ошибка");
                    return;
                }

                UserSession.Instance.Role = user.Role.Name;
                PageHandler.NavigateToProductListPage();
            }
            catch (InvalidOperationException) //Защита от слишком быстрых действий
            {
            }
            catch (Exception ex)
            {
                ShowError(ex.Message, "Непредвиденная ошибка");
                return;
            }
        }

        private static void ShowError(string text, string title)
        {
            MessageBox.Show(text,title, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private bool CheckUserInput()
            => !String.IsNullOrWhiteSpace(Login)
                && !String.IsNullOrWhiteSpace(Password);

        public event PropertyChangedEventHandler? PropertyChanged; //Реализация интерфейса INotifyPropertyChanged
        public void OnPropertyChanged([CallerMemberName] string prop = "") // Метод для вызова события PropertyChanged с именем изменившегося свойства
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
