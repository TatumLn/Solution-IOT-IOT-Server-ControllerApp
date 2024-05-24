using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOT_Controller.ViewsModels
{
    public class SettingIndicatorSelector : DataTemplateSelector
    {
        public DataTemplate? LastIndicateur { get; set; }
        public DataTemplate? DefaultIndicateur { get; set; }

        protected override DataTemplate? OnSelectTemplate(object item, BindableObject container)
        {
            return ((CarrouselModels)item).IsLast ? LastIndicateur : DefaultIndicateur;
        }
    }
}
