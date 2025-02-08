using Første_SQL.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Første_SQL.ViewModels
{

    public class ManageUserViewModel : INotifyPropertyChanged
    {
        private UserRepository _userRepository;
        public ObservableCollection<User> Users { get; set; }

        private User _selectedUser;
        public User SelectedUser
        {
            get => _selectedUser;
            set
            {
                if (_selectedUser != value)
                {
                    _selectedUser = value;
                    OnPropertyChanged(nameof(SelectedUser));
                }
            }
        }


        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        
        public ManageUserViewModel()
        {
            _userRepository = new UserRepository();
            Users = new ObservableCollection<User>();

            RefreshList();

        }

        public void AddUser()
        {
            User newUser = new User()
            {
                Username = this.UserName,
                Password = this.Password,
                IsAdmin = this.IsAdmin,
            };

            if (newUser != null)
            {
                _userRepository.Add(newUser);
                Users.Add(newUser);
            }
        }

        public void RemoveUser(User userToBeDeleted)
        {
            if (userToBeDeleted != null)
            {                
               _userRepository.Delete(userToBeDeleted.Id);
               Users.Remove(userToBeDeleted);

            }
        }

        public void UpdateUser(User user)
        {
            if (user != null)
            {
                _userRepository.Update(user);
                RefreshList();
            }
        }

        public void RefreshList()
        {
            foreach (User user in _userRepository.RetrieveAll())
            {
                Users.Add(user);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string propertyname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }
    }
}
