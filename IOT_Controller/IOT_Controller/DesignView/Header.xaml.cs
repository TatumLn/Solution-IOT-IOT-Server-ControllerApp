using IOT_Controller.ViewsModels;

namespace IOT_Controller.DesignView
{
    public partial class Header : BaseContentView
    {
        public Header()
        {
            InitializeComponent();
            BindingContext = new HeaderViewModel();
        }

    }
}
