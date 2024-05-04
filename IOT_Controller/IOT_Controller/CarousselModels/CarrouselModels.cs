using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOT_Controller.CarousselModels
{
    public class CarrouselModels : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private string[] _pageTexts = { "Reunion", "Bureau", "Jardinage", "" };
        public string PositionText
        {
            get { return _pageTexts[Position]; }
        }


        private List<string> _backgroundImages;
        public List<string> BackgroundImages
        {
            get => _backgroundImages;
            set
            {
                _backgroundImages = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BackgroundImages)));
            }
        }

        private int _position;
        public int Position
        {
            get { return _position; }
            set
            {
                if (_position != value)
                {
                    _position = value;
                    OnPropertyChanged(nameof(Position));
                    OnPropertyChanged(nameof(PositionText));
                }
            }
        }

        public CarrouselModels()
        {
            // Initialisez la liste d'URLs d'images de fond
            BackgroundImages = new List<string>
            {
                "sallereunion2.jpg",
                "backgroundbureau.jpg",
                "backgroundjardin.jpg",
                "backgroundsetting.jpg"
            };
            // Initialiser la page sélectionnée à 0 (première page)
            Position = 0;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
