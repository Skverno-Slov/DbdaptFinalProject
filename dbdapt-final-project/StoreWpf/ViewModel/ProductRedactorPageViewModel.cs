using Microsoft.Win32;
using StoreLib.Contexts;
using StoreLib.Models;
using StoreLib.Services;
using StoreWpf.Commands;
using StoreWpf.Other;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Windows;

namespace StoreWpf.ViewModel
{
    public class ProductRedactorPageViewModel(bool isCreateMode, int? productId = null) : INotifyPropertyChanged
    {
        private static readonly StoreDbContext _context = App.StoreDbContext;

        private readonly ProductService _productService = new(_context);

        private const string ImageFolderName = "Images";

        private Product _product;
        private Supplier _selectedSupplier;
        private Manufacturer _selectedManufacturer;
        private Category _selectedCategory;
        private Unit _selectedUnit;
        private string? _imageName;
        private string? _imagePath;
        private string? _imageSource;
        private Visibility _addButtonVisibilityType;
        private Visibility _changeButtonVisibilityType;
        private string? _selectedImagePath;
        private string? _currentImagePath;
        private string? _oldProductCode;

        private RelayCommand _loadCommand;
        private RelayCommand _addCommand;
        private RelayCommand _setImageCommand;
        private RelayCommand _clearImageCommand;
        private RelayCommand _changeCommand;

        public ObservableCollection<Supplier> Suppliers { get; set; } = new();
        public ObservableCollection<Manufacturer> Manufacturers { get; set; } = new();
        public ObservableCollection<Category> Categories { get; set; } = new();
        public ObservableCollection<Unit> Units { get; set; } = new();

        public RelayCommand LoadCommand
        {
            get => _loadCommand ??= new RelayCommand(async obj => await LoadDataAsync());
        }

        public RelayCommand AddCommand
        {
            get => _addCommand ??= new RelayCommand(async obj => await AddProductAsync(),
                obj => CheckDataValid());
        }

        public RelayCommand ChangeCommand
        {
            get => _changeCommand ??= new RelayCommand(async obj => await ChangeProductAsync(),
                obj => CheckDataValid());
        }

        public RelayCommand SetImageCommand
        {
            get => _setImageCommand ??= new RelayCommand(obj => SetImage());
        }

        public RelayCommand ClearImageCommand
        {
            get => _clearImageCommand ??= new RelayCommand(obj => ClearImage());
        }

        public Product Product
        {
            get => _product;
            set
            {
                _product = value;
                OnPropertyChanged();
            }
        }

        public Supplier SelectedSupplier
        {
            get
                => _selectedSupplier;

            set
            {
                _selectedSupplier = value;

                if (value != null && Product != null)
                {
                    Product.Supplier = value;
                    Product.SupplierId = value.SupplierId;
                }

                OnPropertyChanged();
            }
        }

        public Manufacturer SelectedManufacturer
        {
            get => _selectedManufacturer;
            set
            {
                _selectedManufacturer = value;

                if (value != null && Product != null)
                {
                    Product.Manufacturer = value;
                    Product.ManufacturerId = value.ManufacturerId;
                }

                OnPropertyChanged();
            }
        }

        public Category SelectedCategory
        {
            get => _selectedCategory;

            set
            {
                _selectedCategory = value;

                if (value != null && Product != null)
                {
                    Product.Category = value;
                    Product.CategoryId = value.CategoryId;
                }

                OnPropertyChanged();
            }
        }

        public Unit SelectedUnit
        {
            get => _selectedUnit;

            set
            {
                _selectedUnit = value;

                if (value != null && Product != null)
                {
                    Product.Unit = value;
                    Product.UnitId = value.UnitId;
                }
                OnPropertyChanged();
            }
        }

        public string? ImagePath
        {
            get => _imagePath;
            set
            {
                _imagePath = value;
                OnPropertyChanged();
            }
        }

        public string? ImageSource
        {
            get => _imageSource;
            set
            {
                _imageSource = value;
                OnPropertyChanged();
            }
        }

        public string? ImageName
        {
            get => _imageName;
            set
            {
                _imageName = value;
                OnPropertyChanged();
            }
        }

        public Visibility AddButtonVisibilityType
        {
            get => _addButtonVisibilityType;
            set
            {
                _addButtonVisibilityType = value;
                OnPropertyChanged();
            }
        }
        public Visibility ChangeButtonVisibilityType
        {
            get => _changeButtonVisibilityType;
            set
            {
                _changeButtonVisibilityType = value;
                OnPropertyChanged();
            }
        }


        private void SetImage()
        {
            try
            {
                var dialog = new OpenFileDialog();
                dialog.Filter = "PNG |*.png|JPEG |*.jpg;*.jpeg|Bitmap |*.bmp|Все файлы (*.*)|*.*";

                if (dialog.ShowDialog() is true)
                {
                    _selectedImagePath = dialog.FileName;

                    ImageName = dialog.SafeFileName;

                    ImagePath = dialog.FileName;

                    ImageSource = dialog.FileName;
                }
            }
            catch (Exception ex)
            {
                ShowError(ex.Message, "Непредвиденная ошибка");
            }
        }

        private void ClearImage()
        {
            _selectedImagePath = null;
            ImageName = null;
            ImagePath = null;
            ImageSource = null;
            Product.Photo = null;
        }

        private async Task CopyImageToImagesFolder()
        {
            if (string.IsNullOrEmpty(_selectedImagePath))
                return;

            try
            {
                string imagesFolder = Path.Combine(Directory.GetCurrentDirectory(), ImageFolderName);
                if (!Directory.Exists(imagesFolder))
                {
                    Directory.CreateDirectory(imagesFolder);
                }

                string fileName = GenerateUniqueFileName(Path.GetFileName(_selectedImagePath));
                string destinationPath = Path.Combine(imagesFolder, fileName);

                File.Copy(_selectedImagePath, destinationPath, false);

                Product.Photo = fileName;

                _currentImagePath = destinationPath;
                ImageSource = destinationPath;

                _selectedImagePath = null;
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка при копировании изображения: {ex.Message}", "Ошибка");
            }
        }

        private string GenerateUniqueFileName(string originalFileName)
        {
            string imagesFolder = Path.Combine(Directory.GetCurrentDirectory(), ImageFolderName);
            string fileNameWithoutExt = Path.GetFileNameWithoutExtension(originalFileName);
            string extension = Path.GetExtension(originalFileName);

            string newFileName = originalFileName;
            int counter = 1;

            while (File.Exists(Path.Combine(imagesFolder, newFileName)))
            {
                newFileName = $"{fileNameWithoutExt}_{counter}{extension}";
                counter++;
            }

            return newFileName;
        }

        private async Task LoadDataAsync()
        {
            try
            {
                await LoadSuppliersAsync();
                await LoadManufacturersAsync();
                await LoadCategoriesAsync();
                await LoadUnitsAsync();

                if (isCreateMode)
                {
                    Product = new();
                    AddButtonVisibilityType = Visibility.Visible;
                    ChangeButtonVisibilityType = Visibility.Collapsed;
                }
                else
                {
                    await LoadProductAsync();
                    AddButtonVisibilityType = Visibility.Collapsed;
                    ChangeButtonVisibilityType = Visibility.Visible;
                }

                if (!String.IsNullOrEmpty(Product.Photo))
                {
                    string imagePath = Path.Combine(Directory.GetCurrentDirectory(), ImageFolderName, Product.Photo);
                    if (File.Exists(imagePath))
                    {
                        _currentImagePath = imagePath;
                        ImageSource = imagePath;
                        ImagePath = imagePath;
                        ImageName = Product.Photo;
                    }
                }
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

        private async Task LoadProductAsync()
        {
            if(!productId.HasValue)
                return;

            Product = await _productService.GetProductAsync(productId.Value);

            if (Product != null)
            {
                SelectedSupplier = Product.Supplier;
                SelectedManufacturer = Product.Manufacturer;
                SelectedCategory = Product.Category;
                SelectedUnit = Product.Unit;

                _oldProductCode = Product.ProductCode;
                if (!string.IsNullOrEmpty(Product.Photo))
                {
                    ImageSource = Product.Photo;
                }
            }
        }

        private async Task LoadUnitsAsync()
        {
            Units?.Clear();

            var units = await _productService.GetUnitsAsync();
            foreach (var unit in units)
                Units.Add(unit);
        }

        private async Task LoadCategoriesAsync()
        {
            Categories?.Clear();

            var categories = await _productService.GetCategoriesAsync();
            foreach (var category in categories)
                Categories.Add(category);
        }

        private async Task LoadManufacturersAsync()
        {
            Manufacturers?.Clear();

            var manufacturers = await _productService.GetManufacturersAsync();
            foreach (var manufacturer in manufacturers)
                Manufacturers.Add(manufacturer);
        }

        private async Task LoadSuppliersAsync()
        {
            Suppliers?.Clear();

            var suppliers = await _productService.GetSuppliersAsync();
            foreach (var supplier in suppliers)
                Suppliers.Add(supplier);
        }

        private async Task AddProductAsync()
        {
            try
            {
                if (_productService.ProductCodeExists(Product.ProductCode))
                {
                    ShowError("Артикул уже существует", "Не сохранено");
                    return;
                }

                if (!CheckDataValid())
                {
                    ShowError("Заполните поля, подсвеченные красным", "Не сохранено");
                    return;
                }

                await CopyImageToImagesFolder();

                await _productService.AddProductAsync(Product);

                PageHandler.NavigateToProductListPage();
            }
            catch (InvalidOperationException)
            {
            }
            catch (Exception ex)
            {
                ShowError(ex.Message, "Непредвиденная ошибка");
            }
        }

        private async Task ChangeProductAsync()
        {
            try
            {
                if(_oldProductCode != Product.ProductCode 
                    && _productService.ProductCodeExists(Product.ProductCode))
                {
                    ShowError("Артикул уже используется другим товаром", "Не сохранено");
                    return;
                }

                if (!CheckDataValid())
                {
                    ShowError("Заполните поля, подсвеченные красным", "Не сохранено");
                    return;
                }

                if (!string.IsNullOrEmpty(_selectedImagePath))
                    await CopyImageToImagesFolder();

                await _productService.ChangeProductAsync(Product);

                PageHandler.NavigateToProductListPage();
            }
            catch (InvalidOperationException)
            {
            }
            catch (Exception ex)
            {
                ShowError(ex.Message, "Непредвиденная ошибка");
            }
        }

        private bool CheckDataValid()
        {
            try
            {
                if (Product == null)
                    return false;

                if (String.IsNullOrWhiteSpace(Product.ProductName))
                    return false;

                if (String.IsNullOrWhiteSpace(Product.ProductCode))
                    return false;

                if (Product.ProductCode.Length > 6)
                    return false;

                if (Product.ProductCode.Length < 6)
                    return false;

                if (!_productService.CheckDiscountValid(Product.Discount))
                    return false;

                if (!_productService.SupplierExists(Product.SupplierId))
                    return false;

                if (!_productService.ManufacturerExists(Product.ManufacturerId))
                    return false;

                if (!_productService.UnitExists(Product.UnitId))
                    return false;

                return _productService.CategorieExists(Product.CategoryId);
            }
            catch (InvalidOperationException)
            {
                return false;
            }
            catch (Exception ex)
            {
                ShowError(ex.Message, "Непредвиденная ошибка");
                return false;
            }
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
