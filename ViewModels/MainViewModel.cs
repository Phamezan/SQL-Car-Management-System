using Første_SQL.Models;
using Første_SQL.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Første_SQL.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly NavigationService _navigationService;
        private CarRepository _carRepository;
        public ObservableCollection<CarViewModel> Cars { get; set; } = new ObservableCollection<CarViewModel>();

        private CarViewModel _selectedCar;
        public CarViewModel SelectedCar
        {
            get => _selectedCar;
            set
            {
                if (_selectedCar != value)
                {
                    _selectedCar = value;
                    OnPropertyChanged(nameof(SelectedCar));
                }
            }
        }

        private string _search;
        public string Search
        {
            get => _search;
            set
            {
                if (_search != value)
                {
                    _search = value;
                    OnPropertyChanged(nameof(Search));
                    SearchButton();
                }
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string propertyname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }

        public MainViewModel(NavigationService navigationService)
        {
            _carRepository = new CarRepository();

            RefreshList();

            SelectedCar = new CarViewModel(new Car());
            _navigationService = navigationService;
        }

        public void AddCar()
        {
            Car newCar = new Car
            {
                Make = SelectedCar.Car.Make,
                Model = SelectedCar.Car.Model,
                Year = SelectedCar.Car.Year,
                Description = SelectedCar.Car.Description
            };
            _carRepository.Create(newCar);

            var newCarVM = new CarViewModel(newCar);
            Cars.Add(newCarVM);

            SelectedCar = new CarViewModel(new Car());

        }

        public void UpdateCar(Car existingCar)
        {
            if (existingCar != null)
            {
                _carRepository.Update(existingCar);
            }
            RefreshList();

        }

        public void DeleteCar(CarViewModel carToBeDeleted)
        {
            _carRepository.Delete(carToBeDeleted.Car.Id);
            Cars.Remove(carToBeDeleted);

            SelectedCar = new CarViewModel(new Car());
        }

        public void RefreshList()
        {
            Cars.Clear();
            foreach (Car car in _carRepository.RetrieveAll())
            {
                var carVM = new CarViewModel(car);
                Cars.Add(carVM);
            }
        }

        public void OpenManageUserWindow()
        {
            _navigationService.OpenWindow<ManageUserWindow>();
            _navigationService.CloseWindow<MainWindow>();
        }

        public void SearchButton()
        {
            if (string.IsNullOrWhiteSpace(Search))
            {
                RefreshList();
            }
            else
            {
                _carRepository.Search(Search);
                Cars.Clear();
                foreach (var car in _carRepository.GetCars())
                {
                    var carVM = new CarViewModel(car);
                    Cars.Add(carVM);
                }
            }
        }

        //RelayCommands til UI Knapper

        public RelayCommand ManageUserWindowCmd => new RelayCommand(execute => OpenManageUserWindow());
        public RelayCommand AddCarCmd => new RelayCommand(execute => AddCar());
        public RelayCommand UpdateCarCmd => new RelayCommand(execute => UpdateCar(SelectedCar.Car), canExecute => SelectedCar != null && !String.IsNullOrEmpty(SelectedCar.Make) && !String.IsNullOrEmpty(SelectedCar.Model) && SelectedCar.Year.HasValue);
        public RelayCommand DeleteCarCmd => new RelayCommand(execute => DeleteCar(SelectedCar), canExecute => SelectedCar != null);
    }
}
