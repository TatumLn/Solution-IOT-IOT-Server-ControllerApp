using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOT_Controller.ViewsModels
{
    public class HeaderViewModel : BaseViewModel
    {
        public required ObservableCollection<string> BtnImgHeader { get; set; }
        private readonly PopUpViewModel _popup;
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

        public HeaderViewModel() 
        {
            BtnImgHeader =
             [
                "notification.svg",
                "power.svg"
             ];
            _popup = new PopUpViewModel();
            UpdateNbrDevice();
            UpdateNbrBtnActive();
        }

        private void UpdateNbrDevice()
        {
            NbrDevice = _popup.NbrBtn;
        }

        private void UpdateNbrBtnActive()
        {
            NbrBtnActive = _popup.NbrBtnActive;
        }
    }
}
