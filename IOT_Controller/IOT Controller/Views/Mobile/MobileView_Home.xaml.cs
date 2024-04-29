using Android.Views;
using IOT_Controller.CarousselModels;
using Java.Math;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;

namespace IOT_Controller.Views.Mobile;

public partial class MobileView_Home : ContentPage
{
    public Grid grid;
    public Border? ControlFrameBordure;
    public RowDefinition rowDefADesactiver;
    public GridLength hauteurTemp;
    public RowDefinition nouveauRowDef;
    public RowDefinitionCollection rowDefinitions;
    public Frame? ControlFrame;

    public MobileView_Home()
	{
        InitializeComponent();
        grid = monGrid;
        rowDefADesactiver = monGrid.RowDefinitions[0];
        hauteurTemp = rowDefADesactiver.Height;
        nouveauRowDef = new RowDefinition { Height = hauteurTemp };
        rowDefinitions = grid.RowDefinitions;
    }

    private void AfficherPlus(object sender, EventArgs e)
    {
        rowDefinitions.RemoveAt(0);// Supprimez le premier RowDefinition
                                   // rowDefinitions[0].Height = new GridLength(1, GridUnitType.Star); // Modifiez la premi�re d�finition de ligne
                                   // D�placer les autres RowDefinitions d'un index vers le haut
                                   // Sauvegarder les hauteurs d'origine de toutes les RowDefinitions
        //monLabel.SetValue(Grid.RowProperty, 1); // D�place monLabelReunion � la deuxi�me ligne
        rowDefinitions.Add(nouveauRowDef);

        //Frames ou contr�les
        var ControlStacklayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center,
                Spacing = 20
            };

        // Cr�ez une boucle pour ajouter plusieurs contr�les � Children
        for (int i = 0; i < 4; i++)
        {
            ControlStacklayout.Children.Add(new Frame
            {
                BackgroundColor = Color.FromArgb("#19CBC0"),
                CornerRadius = 30,
                HeightRequest = 60,
                WidthRequest = 60,
                Margin = 20,
                Content = new Label { Text = $"{i + 1}", 
                                      HorizontalOptions = LayoutOptions.Center
                                    }
            });
        }

        var button = new Button
        {
            CornerRadius = 20,
            BackgroundColor = Color.FromArgb("#EAFD0E"),
            WidthRequest = 80,
            HeightRequest = 40,
            VerticalOptions = LayoutOptions.End,
            Command = new Command(() =>
            {
                AfficherMoins();
            })
        };

        var Mongrille = new Grid
        {
            BackgroundColor = Colors.Transparent,
            RowDefinitions =
                     {
                         new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                         new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                         new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                         new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
                     }
        };

        Mongrille.Children.Add(ControlStacklayout);
        Mongrille.SetRow(ControlStacklayout, 0);
        Mongrille.Children.Add(button);
        Mongrille.SetRow(button, 3);

        ControlFrame = new Frame
        {
            BorderColor = Colors.White,

            Content = Mongrille

        };

        ControlFrameBordure = new Border
        {
            StrokeShape = new RoundRectangle
            {
                CornerRadius = new CornerRadius( 40, 40, 0, 0)
           },
           Content = ControlFrame,
        };

        // Ajoutez le Frame au Grid
        grid.Children.Add(ControlFrameBordure);
        // D�finissez la position du Frame dans le Grid
        Grid.SetRow(ControlFrameBordure, monGrid.RowDefinitions.Count - 1); // Positionnez-le dans le dernier RowDefinition ajout�

    }

    private void AfficherMoins()
    {
        grid.Children.Remove(ControlFrameBordure);

    }
}