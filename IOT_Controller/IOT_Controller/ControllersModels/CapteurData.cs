using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace IOT_Controller.ControllersModels
{
    public class CapteurData
    {
            private string? _temperature;
            public string? Temperature
            {
                get { return _temperature; }
                set { SetProperty(ref _temperature, value); }
            }

            private string? _humidity;
            public string? Humidity
            {
                get { return _humidity; }
                set { SetProperty(ref _humidity, value); }
            }

            private string? _luminosite;
            public string? Luminosite
            {
                get { return _luminosite; }
                set { SetProperty(ref _luminosite, value); }
            }

            private string? _climActif;
            public string? ClimActif
            {
                get => _climActif;
                set { SetProperty(ref _climActif, value); }
            }

            private string? _deshumidActif;
            public string? DeshumidActif
            {
                get => _deshumidActif;
                set { SetProperty(ref _deshumidActif, value); }
            }

            private string? _lumiereActif;
            public string? LumiereActif
            {
                get => _lumiereActif;
                set { SetProperty(ref _lumiereActif, value); }
            }

            protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = "")
            {
                if (Equals(storage, value))
                {
                    return false;
                }

                storage = value;
                OnPropertyChanged(propertyName);
                return true;
            }

            public event PropertyChangedEventHandler? PropertyChanged;
            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
    }
}
