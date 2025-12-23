using Microsoft.Extensions.DependencyInjection;
using StoreLib.Models;
using System.Windows.Controls;

namespace StoreWpf.View.Pages
{
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

        public static void NavigateToRedactorPage(bool isCreateMode = true, int? id = null)
        {
            var page = new ProductRedactorPage(isCreateMode, id);

            CurrentFrame.Navigate(page);
        }
    }
}
