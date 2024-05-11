using IOT_Controller.Views.Mobile;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IOT_Controller;
using System.Globalization;

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


        private List<string> _backgroundImages = new List<string>();
        public List<string> BackgroundImages
        {
            get => _backgroundImages;
            set
            {
                _backgroundImages = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BackgroundImages)));
                OnPropertyChanged(nameof(IsLast));
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
                    OnPropertyChanged(nameof(IsLast));
                }
            }
        }

        // Propriété pour l'IndicatorTemplate
        public IndicatorTemplateSelector? IndicatorTemplate { get; private set; }
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

        public bool IsLast
        {
            get { return Position == BackgroundImages.Count - 1; }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class IndicatorTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DefaultIndicator { get; set; }
        public DataTemplate LastIndicator { get; set; }
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
                return ((CarrouselModels)item).IsLast? LastIndicator : DefaultIndicator;
        }
    }
}

