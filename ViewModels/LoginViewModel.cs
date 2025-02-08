using Første_SQL.Models;
using System.Windows;

namespace Første_SQL.ViewModels
{
    public class LoginViewModel
    {
        private UserRepository _userRepository;
        public string Username { get; set; }
        public string Password { get; set; }
        public LoginViewModel()
        {
            _userRepository = new UserRepository();
        }

        public void Login()
        {
            if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password))
            {
                User loggedInUser = _userRepository.UserLogin(Username, Password);

                if (loggedInUser != null)
                {
                    MainWindow mw = new MainWindow();
                    mw.Show();
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
