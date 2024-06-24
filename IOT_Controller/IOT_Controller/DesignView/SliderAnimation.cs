using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOT_Controller.DesignView
{
     public class SliderAnimation(double initialValue, Action<double> onUpdate)
    {
        private double _currentValue = initialValue;
        private readonly Action<double> _onUpdate = onUpdate;

        public void LancerAnimation(double targetValue)
        {
            var animation = new Animation(valeur =>
            {
                _currentValue = valeur;
                _onUpdate(_currentValue);
            }, _currentValue, targetValue);
            animation.Commit(Application.Current?.MainPage, "Animation", 16, 1000, Easing.Linear);
        }
    }
}
