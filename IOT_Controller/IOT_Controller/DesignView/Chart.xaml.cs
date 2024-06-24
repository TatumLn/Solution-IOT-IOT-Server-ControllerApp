using IOT_Controller.ViewsModels;
using IOT_Controller.API;
using Microcharts.Maui;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace IOT_Controller.DesignView
{
    public partial class Chart : BaseContentView
    {
       public Chart() 
        {
            InitializeComponent();
            InitializeGesture();
        }

        private void InitializeGesture()
        {
            /*var _taperHorsChart = new TapGestureRecognizer();
            _taperHorsChart.Tapped += (s, e) => 
            {
                FermerChart(s,e); 
            };
            GestureGrid.GestureRecognizers.Add(_taperHorsChart);*/
            //
            var _swipeVersleBas = new SwipeGestureRecognizer { Direction = SwipeDirection.Down };
            _swipeVersleBas.Swiped += (s, e) => { FermerChart(s?? "", e); };
            GestureGrid.GestureRecognizers.Add(_swipeVersleBas);
        }

        private void FermerChart(object sender, EventArgs e)
        {
            MainViewModel.Instance.IsChartVisible = false;
        }

    }
}
