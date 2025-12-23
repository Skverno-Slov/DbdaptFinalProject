using Microsoft.EntityFrameworkCore;
using StoreLib.Contexts;
using StoreLib.DTOs;
using StoreLib.Models;
using StoreLib.Services;
using StoreWpf.Commands;
using StoreWpf.Other;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Windows;

namespace StoreWpf.ViewModel
{
    public class ProductListPageViewModel : INotifyPropertyChanged
    {
        private static readonly StoreDbContext _context = App.StoreDbContext;

        private readonly ProductService _productService = new(_context);

        private RelayCommand _loadCommand;
        private RelayCommand _updateCommand;
        private RelayCommand _deleteCommand;
        private RelayCommand _navigetToUpdatePageCommand;
        private RelayCommand _navigateToAddPageCommand;

        private ProductCardDto? _selectedProduct;
        private string? _productDescription;
        private string? _selectedtManufacturer;
        private string? _selectedSortColumn;
        private decimal? _maxPrice;
        private bool _isInStock;
        private bool _isDiscounted;

        public ObservableCollection<ProductCardDto>? ProductCards { get; set; } = new(); //ObservableCollection - отслеживаемый список

        public ObservableCollection<string> Manufacturers { get; set; } = new();

        public ObservableCollection<string> SortColumns { get; set; } = new();

        public RelayCommand LoadCommand
        {
            get => _loadCommand ??= new RelayCommand(async obj => await LoadDataAsync());
        }

        public RelayCommand UpdateCommand
        {
            get => _updateCommand ??= new RelayCommand(async obj => await UpadteDataAsync());
        }

        public RelayCommand DeleteCommand
        {
            get => _deleteCommand ??= new RelayCommand(async obj => await DeleteProductAsync(),
                obj => IsProductSelected() && CheckRole());
        }

        public RelayCommand NavigetToUpdatePageCommand
        {
            get => _navigetToUpdatePageCommand ??= new RelayCommand(obj => NavigteToRedactorPageInChangeMode(),
                obj => IsProductSelected() && CheckRole());
        }

        public RelayCommand NavigetToAddPageCommand
        {
            get => _navigateToAddPageCommand ??= new RelayCommand(obj => NavigteToRedactorPageInCreateMode(),
                obj => CheckRole());
        }

        public ProductCardDto? SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged();
            }
        }

        public string? ProductDescription
        {
            get => _productDescription;
            set
            {
                _productDescription = value;
                OnPropertyChanged();

                UpdateCommand.Execute(null); //обновление после изменения (из за отсутсвия возможности привязать команду)
            }
        }

        public string? SelectedSortColumn
        {
            get => _selectedSortColumn;
            set
            {
                _selectedSortColumn = value;
                OnPropertyChanged();

                UpdateCommand.Execute(null);
            }
        }

        public string? SelectedManufacturer
        {
            get => _selectedtManufacturer;
            set
            {
                _selectedtManufacturer = value;
                OnPropertyChanged();

                UpdateCommand.Execute(null);
            }
        }

        public decimal? MaxPrice
        {
            get => _maxPrice;
            set
            {
                _maxPrice = Convert.ToDecimal(value);
                OnPropertyChanged();

                UpdateCommand.Execute(null);
            }
        }

        public bool IsInStock
        {
            get => _isInStock;
            set
            {
                _isInStock = value;
                OnPropertyChanged();

                UpdateCommand.Execute(null);
            }
        }

        public bool IsDiscounted
        {
            get => _isDiscounted;
            set
            {
                _isDiscounted = value;
                OnPropertyChanged();

                UpdateCommand.Execute(null);
            }
        }

        private void NavigteToRedactorPageInChangeMode()
        {
            PageHandler.NavigateToRedactorPage(false, SelectedProduct.ProductId); //Открыти страницы редактора в режиме изменения
        }

        private void NavigteToRedactorPageInCreateMode()
        {
            PageHandler.NavigateToRedactorPage(); //Открыти страницы редактора в режиме добавления
        }

        private bool IsProductSelected()
            => SelectedProduct is not null;

        private bool CheckRole()
        {
            var role = UserSession.Instance.Role;
            return role == "Администратор" || role == "Менеджер"; //Получение роли текущего пользователя
        }

        private async Task DeleteProductAsync()
        {
            try
            {
                var selectedProduct = await _productService
                    .GetProductAsync(SelectedProduct.ProductId);

                await _productService.DeleteProductAsync(selectedProduct);

                await UpadteDataAsync();
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

        private async Task LoadDataAsync() //первая загрузка
        {
            try
            {
                LoadStaticData(); 
                await LoadManufacturersAsync();

                SelectedSortColumn = "По названию";
                SelectedManufacturer = "Все производители";
                MaxPrice = 0;
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

        private async Task UpadteDataAsync() //обновление
        {
            try
            {
                await LoadProductsAsync();
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

        //загрузка данных не трубующих БД
        private void LoadStaticData()
        {
            var sortColumns = new List<string>()
            {
               "По названию",
               "По поставщику",
               "По цене (дешевые)",
               "По цене (дорогие)"
            };

            foreach (var column in sortColumns)
                SortColumns?.Add(column);
        }

        //Закгрузка производителей  из бд и создание источника данных для выпадающего списка + опция все производители
        private async Task LoadManufacturersAsync()
        {
            Manufacturers?.Clear();

            var manufacturers = await _productService.GetManufacturersAsync();
            Manufacturers?.Add("Все производители");

            foreach (var manufacturer in manufacturers.Select(m => m.Name))
                Manufacturers?.Add(manufacturer);
        }

        // загрузка товаров и применение фильтров -> сортировок
        private async Task LoadProductsAsync()
        {
            string? manufacrurerName = NormalizeData();

            var products = _productService.GetProductCards();

            //фильтры
            products = _productService.ApplyDescriptionFilter(ProductDescription, products);
            products = _productService.ApplyManufacturerFilter(manufacrurerName, products);
            products = _productService.ApplyMaxPriceFilter(MaxPrice, products);
            products = _productService.ApplyInStockFilter(IsInStock, products);
            products = _productService.ApplyDiscountedFilter(IsDiscounted, products);
            //сортировка
            products = _productService.ApplySorting(SelectedSortColumn, products);

            var productsList = await products.ToListAsync();

            //обновление списка
            ProductCards?.Clear();
            foreach (var product in productsList)
                ProductCards?.Add(product);
        }

        //метод нужен для корректной работы ApplyManufacturerFilter
        private string? NormalizeData()
        {
            string? manufacrurerName;
            if (SelectedManufacturer == "Все производители")
                manufacrurerName = ""; //если выбраны "Все производители" отправлять в ApplyManufacturerFilter пустую строку
            else
                manufacrurerName = SelectedManufacturer;
            return manufacrurerName;
        }

        private static void ShowError(string text, string title)
        {
            MessageBox.Show(text, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
