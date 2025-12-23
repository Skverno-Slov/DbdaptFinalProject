using StoreLib.Models;
using StoreWpf.ViewModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace StoreWpf.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProductRedactorPage.xaml
    /// </summary>
    public partial class ProductRedactorPage : Page
    {
        ProductRedactorPageViewModel _viewModel;

        public ProductRedactorPage(bool isCreateMode, int? productId = null)
        {
            InitializeComponent();
            _viewModel = new ProductRedactorPageViewModel(isCreateMode, productId);

            DataContext = _viewModel;
        }

        private void ProductRedactorPage_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel.LoadCommand.Execute(null);
        }

        private void DiscountTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
                return;
            }
        }

        private void QuantityTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }

        private void PriceTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            var regex = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");
            var text = PriceTextBox.Text + e.Text;

            if (!regex.IsMatch(text) || (e.Text == "." && PriceTextBox.Text.Contains(".")))
            {
                e.Handled = true;
            }
        }
    }
}
