using StoreLib.Contexts;
using System.Configuration;
using System.Data;
using System.Windows;

namespace StoreWpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static StoreDbContext StoreDbContext { get; } = new();
    }

}
