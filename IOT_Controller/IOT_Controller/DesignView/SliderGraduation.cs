using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using Microsoft.Maui.Controls;
using System.ComponentModel;

namespace IOT_Controller.DesignView
{
    public class SliderGraduation : SkiaSharp.Views.Maui.Controls.SKCanvasView, INotifyPropertyChanged
    {
        private double _value = 20; // Valeur par défaut

        public static readonly BindableProperty ValueProperty =
                BindableProperty.Create(nameof(Value), typeof(double), typeof(SliderGraduation), 10.0, propertyChanged: OnValueChanged);

            public double Value
            {
                get => (double)GetValue(ValueProperty);
                set => SetValue(ValueProperty, value);
            }

            private static void OnValueChanged(BindableObject bindable, object oldValue, object newValue)
            {
                var control = (SliderGraduation)bindable;
                control.InvalidateSurface(); // Redraw the canvas
            }

            public SliderGraduation()
            {
                PaintSurface += OnPaintSurface;
            // Abonnement à l'événement ValueChanged du canvas du cercle
            SliderEndPoint.SliderValueChanged += OnSliderValueChanged;
        }

        // Méthode appelée lorsque la valeur du slider change dans le canvas du cercle
        private void OnSliderValueChanged(object sender, double nouveauValue)
        {
            _value = Math.Round(nouveauValue); // Met à jour la valeur affichée dans ce canvas de graduation
            InvalidateSurface(); // Redessine le canvas pour refléter la nouvelle valeur
        }

        protected void OnPaintSurface(object? sender, SKPaintSurfaceEventArgs e)
        {

            var canvas = e.Surface.Canvas;
            canvas.Clear(SKColors.Transparent);

            var width = e.Info.Width;
            var height = e.Info.Height;
            var centerX = width / 2;
            var centerY = height / 2;

            var totalDisplayedValues = 5; // Nombre total de valeurs affichées
            var middleIndex = totalDisplayedValues / 2; // Index de la valeur au milieu

            var minValue = Math.Max(10, (int)_value - middleIndex);
            var maxValue = Math.Min(30, (int)_value + middleIndex);

            var spacing = 80; // Espacement entre chaque valeur

            using (var textPaint = new SKPaint
            {
                Color = SKColors.Gray,
                TextSize = 50,
                FakeBoldText = false,
                IsAntialias = true,
                TextAlign = SKTextAlign.Center
            })
            {
                for (int i = minValue; i <= maxValue; i++)
                {
                    var value = i;
                    var positionFromCenter = value - (int)_value;
                    var x = centerX + positionFromCenter * spacing;
                    textPaint.Color = (value == (int)_value) ? SKColor.Parse("#FA5151") : SKColor.Parse("#000000"); // Met en surbrillance la valeur courante en rouge
                    canvas.DrawText(value.ToString("0"), x, centerY, textPaint);
                }
            }

        }
    }
}