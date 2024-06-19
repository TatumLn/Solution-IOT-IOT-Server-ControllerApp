using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace IOT_Controller.ViewsModels
{
    public class SliderViewModel : BaseViewModel
    {
        private double _sliderValue;

        public double SliderValue
        {
            get => _sliderValue;
            set
            {
                _sliderValue = value;
                OnPropertyChanged(nameof(SliderValue));
            }
        }
    }
}
