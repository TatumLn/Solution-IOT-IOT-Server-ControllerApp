using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IOT_Controller.ViewsModels
{
    public class HeaderViewModel : BaseViewModel
    {
        public ObservableCollection<HeaderButtonViewModel> BtnImgHeader { get; set; }
        private PopUpViewModel _popup;
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
            _popup = new PopUpViewModel();
            BtnImgHeader =
            [
                new() {
                    ImageSource = "notification.svg",
                    Command = new Command(OnNotificationClicked)
                },
                new() {
                    ImageSource = "power.svg",
                    Command = new Command(OnPowerClicked)
                }
            ];
  
            UpdateNbrDevice();
            UpdateNbrBtnActive();

            _popup.PropertyChanged += PopUp_PropertyChanged;

        }

        private void PopUp_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(PopUpViewModel.NbrBtn))
            {
                UpdateNbrDevice();
            }
            else if (e.PropertyName == nameof(PopUpViewModel.NbrBtnActive))
            {
                UpdateNbrBtnActive();
            }
        }

        private void UpdateNbrDevice()
        {
            NbrDevice = _popup.NbrBtn;
        }

        private void UpdateNbrBtnActive()
        {
            NbrBtnActive = _popup.NbrBtnActive;
        }

        private void OnNotificationClicked()
        {
            // Logique pour le bouton de notification
            Console.WriteLine("Notification button clicked");
        }

        private void OnPowerClicked()
        {
            // Logique pour le bouton d'alimentation
            Console.WriteLine("Power button clicked");
        }
    }


    /*#######################################################################################################################################################################
     *  Classe HeaderBoutton
     *  ####################################################################################################################################################################*/

    public class HeaderButtonViewModel
    {
        public string ImageSource { get; set; }
        public ICommand Command { get; set; }
    }
}
