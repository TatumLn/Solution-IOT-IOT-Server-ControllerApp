using IOT_Controller.ViewsModels;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace IOT_Controller.DesignView
{
    public partial class Header : BaseContentView
    {
        public ObservableCollection<string> BtnImgHeader { get; set; }
        private readonly PopUp _popup;
        private int _nbrDevice;
        public int NbrDevice
        {
            get { return _nbrDevice; }
            set
            {
                _nbrDevice = value;
                OnPropertyChanged(nameof(NbrDevice));
            }
        }

        private int _nbrBtnActive;
        public int NbrBtnActive
        {
            get { return _nbrBtnActive; }
            set
            {
                _nbrBtnActive = value;
                OnPropertyChanged(nameof(NbrBtnActive));
            }
        }
        public Header()
        {
            InitializeComponent();
            BtnImgHeader =
            [
                "notification.svg",
                "power.svg"
            ];
            _popup = new PopUp();
            UpdateNbrDevice();
            UpdateNbrBtnActive();
            this.BindingContext = this;
        }

        private void UpdateNbrDevice()
        {
            NbrDevice = _popup.NbrBtn;
        }

        private void UpdateNbrBtnActive()
        {
            NbrBtnActive = _popup.NbrBtnActive;
        }

        public override void OnMqttTopicRecu(string topic, string payload)
        {
            base.OnMqttTopicRecu(topic, payload);
            // Gérer le message MQTT reçu ici
        }

    }
}
