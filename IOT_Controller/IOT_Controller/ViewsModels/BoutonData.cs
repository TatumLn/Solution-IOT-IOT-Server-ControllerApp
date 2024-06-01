using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IOT_Controller.ViewsModels
{
    public class BoutonData : INotifyPropertyChanged
    {
        public string? ButtonText { get; set; }
        public ICommand? ButtonCommand { get; set; }
        public Color? BackgroundColor { get; set; }
        public String? ButtonImage { get; set; }
        public int ButtonIndex { get; set; }
        public string? ButtonState { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        public void UpdateBackgroundColor()
        {
            if (ButtonState == "1")
            {
                // Mettre à jour l'arrière-plan en noir si ButtonState est vrai
                BackgroundColor = Color.FromArgb("000000");
            }
            else if (ButtonState == "")
            {
                ButtonState = "Vrai";
            }
            // Ajoutez d'autres conditions pour d'autres états si nécessaire
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
