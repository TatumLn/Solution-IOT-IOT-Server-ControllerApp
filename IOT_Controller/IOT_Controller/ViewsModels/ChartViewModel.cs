using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microcharts;
using Microcharts.Maui;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using IOT_Controller.API;

namespace IOT_Controller.ViewsModels
{
    public class ChartViewModel: BaseViewModel
    {
        private MainViewModel _mqttConnexion;
        private LineChart _temperatureChart;
        private LineChart _humidityChart;
        private LineChart _luminosityChart;

        public LineChart TemperatureChart
        {
            get => _temperatureChart;
            set
            {
                _temperatureChart = value;
                OnPropertyChanged();
            }
        }

        public ChartViewModel()
        {
            var temperatureEntries = new ObservableCollection<ChartEntry>();

            var random = new Random();
            for (int i = 0; i < 24; i++)
            {
                var temperature = random.Next(15, 30);
                temperatureEntries.Add(new ChartEntry(temperature)
                {
                    Label = $"{i}h",
                    ValueLabel = temperature.ToString(),
                    Color = SKColor.Parse("#FF5757")
                });
            }

            TemperatureChart = new LineChart 
            { 
                Entries = temperatureEntries,
                LabelTextSize = 35,
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal,
                LineMode = LineMode.Spline,
                LineSize = 8,
                PointMode = PointMode.Circle,
                BackgroundColor = SKColors.Transparent
            };
        }
    }
 }
