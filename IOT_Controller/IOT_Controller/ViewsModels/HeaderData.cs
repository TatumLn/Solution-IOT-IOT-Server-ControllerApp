using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOT_Controller.ViewsModels
{
    public class HeaderData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public ObservableCollection<string> BtnImgHeader { get; set; }

        private readonly ContenuPopUp _contenuPopUp;
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

        public HeaderData ()
        {
            _contenuPopUp = new ContenuPopUp();
            _contenuPopUp.PropertyChanged += ContenuPopUp_PropertyChanged;

            // Initialisation de l'image des boutons
            BtnImgHeader =
            [
                "notification.svg",
                "power.svg"
            ];

            //Mise a jour 
            UpdateNbrDevice();
            UpdateNbrBtnActive();
        }

        private void ContenuPopUp_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ContenuPopUp.nbrBtn))
            {
                UpdateNbrDevice();
            }
            if (e.PropertyName == nameof(ContenuPopUp.nbrBtnActive))
            {
                UpdateNbrBtnActive();
            }
        }

        private void UpdateNbrDevice()
        {
            NbrDevice = _contenuPopUp.nbrBtn;
        }

        private void UpdateNbrBtnActive()
        {
            NbrBtnActive = _contenuPopUp.nbrBtnActive;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
