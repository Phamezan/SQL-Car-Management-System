using Første_SQL.Models;
using Første_SQL.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Første_SQL.ViewModels
{

    public class ManageUserViewModel : INotifyPropertyChanged
    {
        private readonly NavigationService _navigationService;
        private UserRepository _userRepository;
        public ObservableCollection<UserViewModel> Users { get; set; } = new ObservableCollection<UserViewModel>();

        private UserViewModel _selectedUser;
        public UserViewModel SelectedUser
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
        
        public ManageUserViewModel(NavigationService navigationService)
        {
            _userRepository = new UserRepository();

            RefreshList();

            SelectedUser = new UserViewModel(new User());

            _navigationService = navigationService;
        }

        public void AddUser()
        {
            User newUser = new User()
            {
                Username = SelectedUser.Username,
                Password = SelectedUser.Password,
                IsAdmin = SelectedUser.IsAdmin
            };

            if (newUser != null)
            {
                _userRepository.Add(newUser);
                var newUserVM = new UserViewModel(newUser);
                Users.Add(newUserVM);
            }

            SelectedUser = new UserViewModel(new User());
        }

        public void DeleteUser(UserViewModel userToBeDeleted)
        {
            if (userToBeDeleted != null)
            {                
               _userRepository.Delete(userToBeDeleted.User.Id);
               Users.Remove(userToBeDeleted);

            }
            SelectedUser = new UserViewModel(new User());
        }

        public void UpdateUser(User user)
        {
            if (user != null)
            {
                _userRepository.Update(user);
            }
                RefreshList();
        }

        public void RefreshList()
        {
            Users.Clear();
            foreach (User user in _userRepository.RetrieveAll())
            {
                var UserVM = new UserViewModel(user);
                Users.Add(UserVM);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string propertyname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }

        public void ReturnToMainWindow()
        {
            _navigationService.OpenWindow<MainWindow>();
            _navigationService.CloseWindow<ManageUserWindow>();
        }

        //Relaycommands

        public RelayCommand AddUserCmd => new RelayCommand(execute => AddUser());
        public RelayCommand DeleteUserCmd => new RelayCommand(execute => DeleteUser(SelectedUser), canExecute => SelectedUser != null);
        public RelayCommand UpdateUserCmd => new RelayCommand(execute => UpdateUser(SelectedUser.User), canExecute => SelectedUser != null && !string.IsNullOrEmpty(SelectedUser.Username) && !string.IsNullOrEmpty(SelectedUser.Password));
        public RelayCommand ReturnCmd => new RelayCommand(execute => ReturnToMainWindow());
    }
}
