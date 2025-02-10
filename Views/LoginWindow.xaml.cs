using Første_SQL.ViewModels;
using System.Windows;

namespace Første_SQL.Views
{

    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            NavigationService navigationService = new NavigationService();
            DataContext = new LoginViewModel(navigationService);
        }
    }
}
