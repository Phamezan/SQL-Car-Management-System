using Første_SQL.Models;
using Første_SQL.Views;
using System.Windows;

namespace Første_SQL.ViewModels
{
    public class LoginViewModel
    {
        private readonly NavigationService _navigationService;
        private UserRepository _userRepository;
        public string Username { get; set; }
        public string Password { get; set; }
        public LoginViewModel(NavigationService navigationService)
        {
            _userRepository = new UserRepository();
            _navigationService = navigationService;
        }

        public void Login()
        {
            if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password))
            {
                User loggedInUser = _userRepository.UserLogin(Username, Password);

                if (loggedInUser != null)
                {
                    _navigationService.OpenWindow<MainWindow>();
                    _navigationService.CloseWindow<LoginWindow>();
                }
                else
                {
                    MessageBox.Show("Forkert brugernavn eller adgangskode, prøv igen", "Forkert Login", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }

        public RelayCommand LoginCmd => new RelayCommand(execute => Login());
    }
}
