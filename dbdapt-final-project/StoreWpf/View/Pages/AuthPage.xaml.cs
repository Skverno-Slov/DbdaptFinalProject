using StoreWpf.ViewModel;
using System.Windows.Controls;

namespace StoreWpf.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для AuthWindow.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();

            DataContext = new AuthPageViewModel();
        }

        private void PasswordBox_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            var viewModel = DataContext as AuthPageViewModel;

            viewModel.Password = PasswordBox.Password;
        }
    }
}
