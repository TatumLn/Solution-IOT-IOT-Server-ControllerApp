using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOT_Controller.DesignView
{
    public static class SliderEndPoint
    {
        public static event EventHandler<double> SliderValueChanged;

        public static void PublishSliderValueChanged(double newValue)
        {
            SliderValueChanged?.Invoke(null, newValue);
        }
    }
}
