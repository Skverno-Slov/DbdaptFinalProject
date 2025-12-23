using StoreWpf.ViewModel;
using System.Windows.Controls;

namespace StoreWpf.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для AuthWindow.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        AuthPageViewModel _authPageViewModel;
        public AuthPage()
        {
            InitializeComponent();
            _authPageViewModel = new AuthPageViewModel();

            DataContext = _authPageViewModel; //Создание контекста данных для биндинга (к AuthPageViewModel)
        }

        private void PasswordBox_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            _authPageViewModel.Password = PasswordBox.Password; //Здесь эта типа привязка
        }
    }
}
