using StoreLib.Contexts;
using StoreLib.Services;
using StoreWpf.Commands;
using StoreWpf.View.Pages;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace StoreWpf.ViewModel
{
    public class AuthPageViewModel : INotifyPropertyChanged
    {
        private static readonly StoreDbContext _context = App.StoreDbContext;

        private readonly AuthService _authService = new(_context);
        private readonly UserService _userService = new(_context);

        private string? _login;
        private string? _password;
        private bool _isPasswordVisible;
        private RelayCommand _command;

        public RelayCommand Command
        {
            get => _command ??= new RelayCommand(async obj => await VerifyUser(),
                obj => CheckUserInput());
        }

        public string? Login
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged();
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

                UserSession.Instanse.Role = user.Role.Name;
                PageHandler.NavigateToProductListPage();
            }
            catch (InvalidOperationException)
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

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
