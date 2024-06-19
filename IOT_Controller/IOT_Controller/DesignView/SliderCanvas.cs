using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using Microsoft.Maui.Controls;
using System.ComponentModel;

namespace IOT_Controller.DesignView
{
    public class SliderCanvas : SkiaSharp.Views.Maui.Controls.SKCanvasView, INotifyPropertyChanged
    {
        private float _angle;
        private bool _isDragging;
        private SliderAnimation _sliderAnimation;
        private float _animationWa;

        public static readonly BindableProperty ValueProperty =
            BindableProperty.Create(nameof(Value), typeof(double), typeof(SliderCanvas), 10.0, propertyChanged: OnValueChanged);

        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        private static void OnValueChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = ((SliderCanvas)bindable);
            control._sliderAnimation.LancerAnimation((double)newValue);
        }

        public SliderCanvas()
        {
            EnableTouchEvents = true;
            // Activer les événements de dessin
            PaintSurface += OnPaintSurface;
            Touch += OnTouch;
            _sliderAnimation = new SliderAnimation(10, valeur =>
            {
                _animationWa = (float)((valeur - 10) / 20.0);
                InvalidateSurface();
            });
        }


        private void OnTouch(object? sender, SKTouchEventArgs e)
        {
            if (e.ActionType == SKTouchAction.Pressed || e.ActionType == SKTouchAction.Moved)
            {
                var centerX = CanvasSize.Width / 2;
                var centerY = CanvasSize.Height / 2;
                var dx = e.Location.X - centerX;
                var dy = e.Location.Y - centerY;
                _angle = (float)Math.Atan2(dy, dx);
                Value = (_angle + Math.PI) / (2 * Math.PI) * 100; // Value between 0 and 100
                _isDragging = true;
                InvalidateSurface();
            }
            else if (e.ActionType == SKTouchAction.Released)
            {
                _isDragging = false;
            }

            e.Handled = true;
        }

        protected void OnPaintSurface(object? sender, SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;
            canvas.Clear(SKColors.Transparent);

            var width = e.Info.Width;
            var height = e.Info.Height;
            var centerX = width / 2;
            var centerY = height / 2;
            var radius = Math.Min(width, height) / 2 - 20;

            using (var paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = SKColor.Parse("#0BDA51"),
                StrokeWidth = 10,
                IsAntialias = true
            })
            {
                canvas.DrawCircle(centerX, centerY, radius, paint);
            }

            //Dessiner l'animation  l'eau
            using (var paint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = SKColor.Parse("#0BDA51"),
                IsAntialias = true
            })
            {
                var path = new SKPath();
                path.AddCircle(centerX, centerY, radius);
                canvas.ClipPath(path);

                var waHeight = (float)(centerY + radius * (1 - 2 * _animationWa));
                var rect = new SKRect(centerX - radius, waHeight, centerX + radius, centerY + radius);
                canvas.DrawRect(rect, paint);
            }

            //Dessiner les valeurs autour du cercle
            using (var paint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = SKColors.Black,
                TextSize = 40,
                IsAntialias = true
            })
            { 
                canvas.DrawText("10", centerX - radius - 50, centerY + 15, paint);  //a gauche
                canvas.DrawText("30", centerX + radius + 10, centerY + 15, paint);  //a droite
                canvas.DrawText("20", centerX - 15, centerY - radius - 20, paint);  //en haut
            }

            //Dessiner la valeur actuel du slider
            using (var paint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = SKColors.Black,
                TextSize = 50,
                IsAntialias = true
            })
            {
                var text = Value.ToString("F0");
                var textWidth = paint.MeasureText(text);
                canvas.DrawText(text, centerX - textWidth / 2, centerY + 15, paint);
            }

            //Slider 
            using (var paint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = SKColor.Parse("#0BDA51"),
                IsAntialias = true
            })
            {
                var angle = (float)((Value - 10) / 20 * 2 * Math.PI - Math.PI / 2);
                var handleX = centerX + radius * Math.Cos(angle);
                var handleY = centerY + radius * Math.Sin(angle);
                canvas.DrawCircle((float)handleX, (float)handleY, 15, paint);
            }
        }
    }
}
