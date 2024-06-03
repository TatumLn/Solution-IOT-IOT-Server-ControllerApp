using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IOT_Controller.ViewsModels
{
    public class BoutonViewModel : INotifyPropertyChanged
    {
        public string? ButtonText { get; set; }
        public ICommand? ButtonCommand { get; set; }
        public String? ButtonImage { get; set; }
        public int ButtonIndex { get; set; }
        public string? _buttonState;
        public string? ButtonState
        {
            get => _buttonState;
            set
            {
                _buttonState = value;
                OnPropertyChanged(nameof(ButtonState));
            }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
                UpdateBackgroundColor();
            }
        }

        private string? _borderBackgroundColor;
        public string? BorderBackgroundColor
        {
            get { return _borderBackgroundColor; }
            set
            {
                _borderBackgroundColor = value;
                OnPropertyChanged(nameof(BorderBackgroundColor));
            }
        }

        public void UpdateBackgroundColor()
        {
            // Mettre à jour l'arrière-plan
            BorderBackgroundColor = IsSelected ? "#EAFD0E" : "#3319CBC0";
        }

        public void UpdateButtonState(string state)
        {
            ButtonState = state == "1" ? "actif" : "inactif";
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
