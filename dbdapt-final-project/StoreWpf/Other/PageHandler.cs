using Microsoft.Extensions.DependencyInjection;
using StoreLib.Models;
using StoreWpf.View.Pages;
using System.Windows.Controls;

namespace StoreWpf.Other
{
    //Класс для хранения текущей страницы и методов для навигации на другие страницы
    public static class PageHandler
    {
        public static Frame CurrentFrame { get; set; }

        public static void NavigateToProductListPage()
        {
            var page = new ProductsListPage();

            CurrentFrame.Navigate(page);
        }

        public static void NavigateToLogin()
        {
            var page = new AuthPage();

            CurrentFrame.Navigate(page);
        }

        public static void NavigateToRedactorPage(bool isCreateMode = true, int? id = null) //Открытие страницы в разных режимах (добавление товара (по умолчанию) и изменения) id - идент. товара для изменения
        {
            var page = new ProductRedactorPage(isCreateMode, id);

            CurrentFrame.Navigate(page);
        }
    }
}
