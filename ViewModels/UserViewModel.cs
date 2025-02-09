using Første_SQL.Models;
using System.ComponentModel;

namespace Første_SQL.ViewModels
{
    public class UserViewModel : INotifyPropertyChanged
    {
        private User _user;
        public User User => _user;

        public UserViewModel(User user)
        {
            _user = user;
        }


        public string Username
        {
            get => _user.Username;
            set
            {
                if (_user.Username != value)
                {
                    _user.Username = value;
                    OnPropertyChanged(nameof(Username));
                }
            }
        }

        public string Password
        {
            get => _user.Password;
            set
            {
                if (_user.Password != value)
                {
                    _user.Password = value;
                    OnPropertyChanged(nameof(Password));
                }
            }
        }

        public bool IsAdmin
        {
            get => _user.IsAdmin;
            set
            {
                if (_user.IsAdmin != value)
                {
                    _user.IsAdmin = value;
                    OnPropertyChanged(nameof(IsAdmin));
                }
            }
        }





        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string propertyname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }
    }
}
