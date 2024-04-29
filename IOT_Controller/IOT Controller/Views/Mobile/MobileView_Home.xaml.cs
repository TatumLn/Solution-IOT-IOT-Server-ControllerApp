using IOT_Controller.CarousselModels;
using Microsoft.Maui.Controls;

namespace IOT_Controller.Views.Mobile;

public partial class MobileView_Home : ContentPage
{
    public Grid grid;
    public RowDefinition rowDefASupprimer;
    public GridLength hauteurTemp;
    public RowDefinition nouveauRowDef;
    public RowDefinitionCollection rowDefinitions;

    public MobileView_Home()
	{
        InitializeComponent();
        grid = monGrid;
        rowDefASupprimer = monGrid.RowDefinitions[0];
        hauteurTemp = rowDefASupprimer.Height;
        nouveauRowDef = new RowDefinition { Height = hauteurTemp };
        rowDefinitions = grid.RowDefinitions;
    }

    private void AfficherPlus(object sender, EventArgs e)
    {
        rowDefinitions.RemoveAt(0);// Supprimez le premier RowDefinition
        // rowDefinitions[0].Height = new GridLength(1, GridUnitType.Star); // Modifiez la première définition de ligne
        //monLabel.SetValue(Grid.RowProperty, 1); // Déplace monLabelReunion à la deuxième ligne
        rowDefinitions.Add(nouveauRowDef);

        //Frames ou contrôles
        var monStacklayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center,
                Spacing = 20
            };

        // Créez une boucle pour ajouter plusieurs contrôles à Children
        for (int i = 0; i < 4; i++) // Remplacez 3 par le nombre de fois que vous voulez répéter
        {
            monStacklayout.Children.Add(new Frame
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
        };


        //
        var monFrame = new Frame
        {
            BorderColor = Colors.White,
            // Autres propriétés
            CornerRadius = 40,
            Content = new Grid
            {
                BackgroundColor = Colors.Blue,
                RowDefinitions =
                     {
                         new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                         new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                         new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                         new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
                     },
                Children =
                {
                   
                }
            }
        };

        // Ajoutez le Frame au Grid
        grid.Children.Add(monFrame);
        // Définissez la position du Frame dans le Grid
        Grid.SetRow(monFrame, monGrid.RowDefinitions.Count - 1); // Positionnez-le dans le dernier RowDefinition ajouté

    }

    private void AfficherMoins()
    {
        //rowDefinitions.Insert(0, new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

    }
}