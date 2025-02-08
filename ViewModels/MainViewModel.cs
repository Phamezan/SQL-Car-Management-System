using Første_SQL.Models;
using System.Collections.ObjectModel;

namespace Første_SQL.ViewModels
{
    public class MainViewModel
    {
        private CarRepository _carRepository;
        public ObservableCollection<CarViewModel> Cars { get; set; } // Liste af Cars som er binded til UI og er opdateret når SQL tabellen ændrer sig.
        public Car Car { get; set; } // Et objekt som vi kan binde til UI
        
        public string Make { get; set; }
        public string Model { get; set; }
        public int? Year { get; set; }
        public string Description { get; set; }
        public CarViewModel SelectedCar { get; set; }
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
        }
        
        public void DeleteCar(CarViewModel carToBeDeleted)
        {
          _carRepository.Delete(carToBeDeleted.Car.Id);
            Cars.Remove(carToBeDeleted);
        }


        //RelayCommands til UI Knapper
        public RelayCommand AddCarCmd => new RelayCommand(execute => AddCar());
        public RelayCommand UpdateCarCmd => new RelayCommand(execute => UpdateCar(SelectedCar.Car));
        public RelayCommand DeleteCarCmd => new RelayCommand(execute => DeleteCar(SelectedCar), canExecute => SelectedCar != null);

    }
}
