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
        public event EventHandler<double> ValueChanged;

        public static readonly BindableProperty ValueProperty =
            BindableProperty.Create(nameof(Value), typeof(double), typeof(SliderCanvas), 10.0, propertyChanged: OnValueChanged);

        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        private static void OnValueChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (SliderCanvas)bindable;
            control._sliderAnimation.LancerAnimation((double)newValue);
            SliderEndPoint.PublishSliderValueChanged((double)newValue); // Met à jour la valeur du slider gradué
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
               double nouveauValue = (_angle + Math.PI) / (2 * Math.PI) * 20 + 10;
                // Mettre à jour la valeur uniquement si elle change
                if (Value != nouveauValue)
                {
                    Value = nouveauValue;
                }

                //_isDragging = true;
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
            canvas.Clear(SKColors.White);

            var width = e.Info.Width;
            var height = e.Info.Height;
            var centerX = width / 2;
            var centerY = height / 2;
            var radius = Math.Min(width, height) / 2 - 40;
            var outerRadius = radius + 20;

            // Dessiner le deuxième cercle (bordure extérieure)
            using (var secondcercle = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = SKColor.Parse("#E8FDEE"),
                StrokeWidth = 10,
                IsAntialias = true
            })
            {
                canvas.DrawCircle(centerX, centerY, outerRadius, secondcercle);
            }

            // Dessiner le cercle principal
            using (var premiercercle = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = SKColor.Parse("##00BF63"),
                StrokeWidth = 10,
                IsAntialias = true
            })
            {
                canvas.DrawCircle(centerX, centerY, radius, premiercercle);
            }

            // Dessiner l'image au centre du premier cercle
            var imageResource = SKBitmap.Decode("Resources/Images/add.png"); 
            if (imageResource != null)
            {
                var imageWidth = imageResource.Width;
                var imageHeight = imageResource.Height;
                var destRect = new SKRect(centerX - imageWidth / 2, centerY - imageHeight / 2, centerX + imageWidth / 2, centerY + imageHeight / 2);
                canvas.DrawBitmap(imageResource, destRect);
            }

            //Dessiner l'animation  l'eau
            using (var animationeau = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = SKColor.Parse("#00BF63"),
                IsAntialias = true
            })
            {
                var path = new SKPath();
                path.AddCircle(centerX, centerY, radius);
                canvas.ClipPath(path);

                var waHeight = (float)(centerY + radius * (1 - 2 * _animationWa));
                var rect = new SKRect(centerX - radius, waHeight, centerX + radius, centerY + radius);
                canvas.DrawRect(rect, animationeau);
            }

            

            //Slider 
            using (var curseur = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = SKColor.Parse("#000000"),
                IsAntialias = true
            })
            {
                var angle = (float)((Value - 10) / 20 * 2 * Math.PI - Math.PI / 2);
                var handleX = centerX + outerRadius * Math.Cos(angle);
                var handleY = centerY + outerRadius * Math.Sin(angle);
                canvas.DrawCircle((float)handleX, (float)handleY, 15, curseur);
            }
        }
    }
}
