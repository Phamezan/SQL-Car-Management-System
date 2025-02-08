﻿using Første_SQL.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Første_SQL.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private CarRepository _carRepository;
        public ObservableCollection<CarViewModel> Cars { get; set; } // Liste af Cars som er binded til UI og er opdateret når SQL tabellen ændrer sig.
        
        public string Make { get; set; }
        public string Model { get; set; }
        public int? Year { get; set; }
        public string Description { get; set; }


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

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string propertyname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }

        public MainViewModel()
        {
            _carRepository = new CarRepository();

            Cars = new ObservableCollection<CarViewModel>();
            foreach (Car car in _carRepository.RetrieveAll())
            {
                var carVM = new CarViewModel(car);
                Cars.Add(carVM);
            }
        }

        public void AddCar() 
        {
            if (!string.IsNullOrEmpty(Make) && 
                !string.IsNullOrEmpty(Model) &&
                Year.HasValue &&
                !string.IsNullOrEmpty(Description))
            {
                Car newCar = new Car
                {
                    Make = this.Make,
                    Model = this.Model,
                    Year = this.Year,
                    Description = this.Description
                };
                _carRepository.Create(newCar);

            var newCarVM = new CarViewModel(newCar);
            Cars.Add(newCarVM);

                Make = string.Empty;
                Model = string.Empty;
                Year = null;
                Description = string.Empty;
            }
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
        }

        public void RefreshList() //Fatter ikke hvorfor Listboxen i UI ikke opdateres når information for en bil ændre sig, så derfor gør denne metode jobbet
        {
            Cars.Clear();
            foreach (Car car in _carRepository.RetrieveAll())
            {
                var carVM = new CarViewModel(car);
                Cars.Add(carVM);
            }
        }


        //RelayCommands til UI Knapper
        public RelayCommand AddCarCmd => new RelayCommand(execute => AddCar());
        public RelayCommand UpdateCarCmd => new RelayCommand(execute => UpdateCar(SelectedCar.Car), canExecute => SelectedCar != null && !String.IsNullOrEmpty(SelectedCar.Make) && !String.IsNullOrEmpty(SelectedCar.Model) && SelectedCar.Year.HasValue);
        public RelayCommand DeleteCarCmd => new RelayCommand(execute => DeleteCar(SelectedCar), canExecute => SelectedCar != null);

    }
}
