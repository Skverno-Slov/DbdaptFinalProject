using StoreLib.Models;
using StoreWpf.ViewModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
            _viewModel.LoadCommand.Execute(null); //загрузка данных (нужна для корректной работы асинхронных методов)
        }

        //Проверка валидности целый чисел в DiscountTextBox
        private void DiscountTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
                return;
            }
        }

        //Проверка валидности целый чисел в QuantityTextBox
        private void QuantityTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }

        //Проверка валидности вещественных чисел в PriceTextBox
        private void PriceTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
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
