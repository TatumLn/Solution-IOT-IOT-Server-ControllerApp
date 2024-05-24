using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace IOT_Controller.ControllersModels
{
    //CapteurData du Circuit 1
    public class CapteurData
    {
            private string? _temperature;
            public string? Temperature
            {
                get { return _temperature; }
                set
                {
                    if (SetProperty(ref _temperature, value))
                    {
                        UpdateData("Temperature", value, Temperature);
                    }
                }
            }

            private string? _humidity;
            public string? Humidity
            {
                get { return _humidity; }
                set
                {
                    if (SetProperty(ref _humidity, value))
                    {
                        UpdateData("Humidite", value, Humidity);
                    }
                }
            }

            private string? _luminosite;
            public string? Luminosite
            {
                get { return _luminosite; }
                set
                {
                    if (SetProperty(ref _luminosite, value))
                    {
                        UpdateData("Luminosite", value, Luminosite);
                    }
                }
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

            private ObservableCollection<TroisIndicateur>? _data;
            public ObservableCollection<TroisIndicateur>? Data
            {
                get { return _data; }
                set { SetProperty(ref _data, value); }
            }

        public CapteurData()
        {
            _data = new ObservableCollection<TroisIndicateur>
            {
                new TroisIndicateur { Text = "Temperature", Valeur = ($"{Temperature}+ °C")??"0", IsTriggered = ClimActif},
                new TroisIndicateur { Text = "Luminosite", Valeur = ($"{Luminosite}+ lx")??"0", IsTriggered = LumiereActif},
                new TroisIndicateur { Text = "Humidite", Valeur = ($"{Humidity}+ %")??"0", IsTriggered = DeshumidActif}
            };
        }

        private void UpdateData(string text, string? valeur, string? isTriggered)
        {
            var item = _data?.FirstOrDefault(d => d.Text == text);
            if (item != null)
            {
                item.Valeur = valeur ?? "0";
                item.IsTriggered = isTriggered;
            }
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
