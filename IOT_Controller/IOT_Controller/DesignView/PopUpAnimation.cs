using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOT_Controller.DesignView
{
    public static class PopUpAnimation
    {
            public static async Task ShowAsync(View view)
            {
                view.IsVisible = true;
                view.Opacity = 0;
                await view.ScaleTo(0.3, 0); 
                await Task.WhenAll(
                    view.ScaleTo(1.05, 400, Easing.SpringOut), 
                    view.FadeTo(1, 400) 
                );
                await view.ScaleTo(1, 200, Easing.SpringOut); 
            }

            public static async Task HideAsync(View view)
            {
                await Task.WhenAll(
                    view.ScaleTo(0.3, 200, Easing.SpringIn),
                    view.FadeTo(0, 200) 
                );
                view.IsVisible = false;
            }
    }
}