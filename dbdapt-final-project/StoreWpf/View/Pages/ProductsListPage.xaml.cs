using StoreWpf.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StoreWpf.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProductsListPage.xaml
    /// </summary>
    public partial class ProductsListPage : Page
    {
        private ProductListPageViewModel _viewModel;

        public ProductsListPage()
        {
            InitializeComponent();

            _viewModel = new ProductListPageViewModel();
            DataContext = _viewModel;
        }

        private void ProductListPage_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel.LoadCommand.Execute(null);
        }

        private void MaxPriceTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");
            var text = MaxPriceTextBox.Text + e.Text;

            if (!regex.IsMatch(text) || (e.Text == "." && MaxPriceTextBox.Text.Contains(".")))
            {
                e.Handled = true;
            }
        }
    }
}
