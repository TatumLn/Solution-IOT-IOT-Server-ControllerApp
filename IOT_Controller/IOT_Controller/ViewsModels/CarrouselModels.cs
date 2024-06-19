﻿using IOT_Controller.Views.Mobile;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;

namespace IOT_Controller.ViewsModels
{
    public class CarrouselModels : BaseViewModel
    {
        private readonly string[] _pageTexts = ["Reunion", "Bureau", "Jardinage", ""];

        public string PositionText
        {
            get { return _pageTexts[Position]; }
        }

        private List<string> _backgroundImages = [];
        public List<string> BackgroundImages
        {
            get => _backgroundImages;
            set
            {
                _backgroundImages = value;
                OnPropertyChanged(nameof(BackgroundImages));
                OnPropertyChanged(nameof(IsLast));
            }
        }

        private int _position;
        public int Position
        {
            get { return _position; }
            set
            {
                if (_position != value)
                {
                    _position = value;
                    OnPropertyChanged(nameof(Position));
                    OnPropertyChanged(nameof(PositionText));
                    OnPropertyChanged(nameof(IsLast));
                    OnPropertyChanged(nameof(IsDefaultPage));
                }
            }
        }

        public CarrouselModels()
        {
            // Initialisation d la liste d'URLs d'images de fond
            BackgroundImages =
            [
                "reunion.jpg",
                "bureau.jpg",
                "jardinage.jpg",
                "backgroundsetting.jpg"
            ];

            // Initialisation de la page sélectionnée à 0 (première page)
            Position = 0;
        }

        public bool IsLast
        {
            get { return Position == BackgroundImages.Count - 1; }
        }

        public bool IsDefaultPage
        {
            get { return Position != BackgroundImages.Count - 1; }
        }
    }
}

