using Første_SQL.ViewModels;
using System.Windows;

namespace Første_SQL
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            NavigationService navigationService = new NavigationService();
            DataContext = new MainViewModel(navigationService);
        }
    }
}