using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOT_Controller.ViewsModels
{
    public class SettingTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? LastPageTemplate { get; set; }
        public DataTemplate? DefaultTemplate { get; set; }

        protected override DataTemplate? OnSelectTemplate(object item, BindableObject container)
        {
            var carousel = container as CarouselView;
            if (carousel == null)
                return DefaultTemplate;

            var position = carousel.ItemsSource.Cast<object>().ToList().IndexOf(item);
            if (position == carousel.ItemsSource.Cast<object>().Count() - 1)
                return LastPageTemplate;

            return DefaultTemplate;
        }
    }
}
