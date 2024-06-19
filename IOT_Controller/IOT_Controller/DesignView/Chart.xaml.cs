using IOT_Controller.ViewsModels;
using IOT_Controller.API;
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
            var _taperHorsChart = new TapGestureRecognizer();
            _taperHorsChart.Tapped += (s, e) => { FermerChart(); };
            this.GestureRecognizers.Add(_taperHorsChart);
            //
            var _swipeVerslaGauche = new SwipeGestureRecognizer { Direction = SwipeDirection.Left };
            _swipeVerslaGauche.Swiped += (s, e) => { FermerChart(); };
            this.GestureRecognizers.Add(_swipeVerslaGauche);
            //
            var _swipeVerslaDroite = new SwipeGestureRecognizer { Direction = SwipeDirection.Right };
            _swipeVerslaDroite.Swiped += (s, e) => { FermerChart(); };
            this.GestureRecognizers.Add(_swipeVerslaDroite);
        }

        private void FermerChart()
        {
            MainViewModel.Instance.IsChartVisible = false;
        }
    }
}
