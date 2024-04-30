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

        public CarrouselModels()
        {
            // Initialisez la liste d'URLs d'images de fond
            BackgroundImages = new List<string>
            {
                "sallereunion2.jpg",
                "sallereunion2.jpg",
                "sallereunion2.jpg",
                "backgroundsetting.jpg"
            };
        }
    }
}
