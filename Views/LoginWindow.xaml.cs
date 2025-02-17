using Første_SQL.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Første_SQL.Views
{

    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            NavigationService navigationService = new NavigationService();
            DataContext = new LoginViewModel(navigationService);

            PasswordBox.PasswordChanged += (object sender, RoutedEventArgs e) =>
            {
                if (DataContext is LoginViewModel loginViewModel)
                {
                    loginViewModel.Password = PasswordBox.Password;
                }
            };
        }
    }
}
