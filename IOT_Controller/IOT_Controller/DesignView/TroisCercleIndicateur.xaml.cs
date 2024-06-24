using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using Microsoft.Maui.Controls;
using IOT_Controller.ViewsModels;
using IOT_Controller.API;
using SkiaSharp.Views.Maui.Controls;

namespace IOT_Controller.DesignView;

public partial class TroisCercleIndicateur : BaseContentView
{
    private SliderAnimation _sliderAnimation;
    private double _animationWa;

    public TroisCercleIndicateur() 
    {
        InitializeComponent();
        var viewModel = (TroisCercleIndicateurViewModel)BindingContext;
        _sliderAnimation = new SliderAnimation(viewModel.CurrentValue, UpdateAnimation);
    }

    private void UpdateAnimation(double value)
    {
        _animationWa = value;
        //canvasView.InvalidateSurface();
    }

    private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {
        var canvas = e.Surface.Canvas;
        canvas.Clear();

        var info = e.Info;
        float centerX = info.Width / 2;
        float centerY = info.Height / 2;
        float radius = Math.Min(info.Width, info.Height) / 2;

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
    }

    private void AfficherChart(object sender, EventArgs e)
    {
        
        MainViewModel.Instance.IsChartVisible = true;

        if (sender is Button button && double.TryParse(button.Text, out double targetValue))
        {
            _sliderAnimation.LancerAnimation(targetValue);
        }
    }
}