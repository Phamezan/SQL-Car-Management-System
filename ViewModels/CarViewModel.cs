using Første_SQL.Models;
using System.ComponentModel;

namespace Første_SQL.ViewModels
{
    public class CarViewModel : INotifyPropertyChanged
    {
        private Car _car;
        public Car Car => _car;
        public CarViewModel(Car car)
        {
            _car = car;
        }
        
        public int? Id
        {
            get => _car.Id;
            set
            {
                if (_car.Id != value)
                {
                    _car.Id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }
    
        public string Make
        {
            get => _car.Make;
            set
            {
                if (_car.Make != value)
                {
                    _car.Make = value;
                    OnPropertyChanged(nameof(Make));
                }
            }
        }

        public string Model
        {
            get => _car.Model;
            set
            {
                if (_car.Model != value)
                {
                    _car.Model = value;
                    OnPropertyChanged(nameof(Model));
                }
            }
        }

        public int? Year
        {
            get => _car.Year;
            set
            {
                if (_car.Year != value)
                {
                    _car.Year = value;
                    OnPropertyChanged(nameof(Year));
                }
            }
        }


        public string Description
        {
            get => _car.Description;
            set
            {
                if (_car.Description != value)
                {
                    _car.Description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}
