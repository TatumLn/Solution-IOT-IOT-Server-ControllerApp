using Microsoft.Maui.Controls.Shapes;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using IOT_Controller.ViewsModels;

namespace IOT_Controller.DesignView;

public partial class PopUp : BaseContentView
{
    public ObservableCollection<BoutonViewModel> ButtonList { get; set; }
    public ICommand AddButtonCommand { get; }
    public string[] _topicStateAppareil = ["iot/climActif", "iot/deshumidActif", "iot/lumiereActif"];
    public string _topicVirtuel = "iot/ValeurControler";
    public int Index { get; set; }

    private string? _selectedContent;
    public string? SelectedContent
    {
        get { return _selectedContent; }
        set
        {
            _selectedContent = value;
            OnPropertyChanged(nameof(SelectedContent));
        }
    }

    private int _selectedButtonIndex;
    public int SelectedButtonIndex
    {
        get { return _selectedButtonIndex; }
        set
        {
            _selectedButtonIndex = value;
            OnPropertyChanged(nameof(SelectedButtonIndex));
        }
    }

    //Compteur 
    public int NbrBtn => (ButtonList.Count) - 1;
    public int NbrBtnActive => GetNbrBtnActive();

    public PopUp()
    {
        InitializeComponent();
        ButtonList = [];
        AddButtonCommand = new Command(AddButton);
        Index = 1;
        // Initialisation des boutons
        Task.Run(() => InitializeButtons());
        this.BindingContext = this;
        InitializeGesture();
    }

    private async void InitializeButtons()
    {
        ButtonList.Add(new BoutonViewModel { ButtonText = "Climatiseur", ButtonIndex = Index++, ButtonImage = "icon_climatisation.png" });
        ButtonList.Add(new BoutonViewModel { ButtonText = "Deshumidificateur", ButtonIndex = Index++, ButtonImage = "icon_deshumidificateur.png" });
        ButtonList.Add(new BoutonViewModel { ButtonText = "Lumiere", ButtonIndex = Index++, ButtonImage = "icon_lampe.png" });
        ButtonList.Add(new BoutonViewModel { ButtonText = "Add", ButtonIndex = Index++, ButtonImage = "icon_ajouter.png" });

        foreach (var button in ButtonList)
        {
            button.ButtonCommand = new Command(async () => await UpdateSelectedContentAsync(button.ButtonIndex));
            if (button.ButtonIndex == _topicStateAppareil.Length)
            {
                var state = await DataTopicStateAsync(_topicStateAppareil[button.ButtonIndex - 1]);
                button.UpdateButtonState(state);
            }
            button.UpdateBackgroundColor();
        }

        OnPropertyChanged(nameof(NbrBtn));
        OnPropertyChanged(nameof(NbrBtnActive));
    }

    private void InitializeGesture()
    {
        var _taperHorsPopUp = new TapGestureRecognizer();
        _taperHorsPopUp.Tapped += (s, e) => { IsVisible = false; };
        this.GestureRecognizers.Add(_taperHorsPopUp);
        //
        var _swipeVersleBas = new SwipeGestureRecognizer { Direction = SwipeDirection.Down };
        _swipeVersleBas.Swiped += (s, e) => { IsVisible = false; };
        this.GestureRecognizers.Add(_swipeVersleBas);
    }

    private async Task UpdateSelectedContentAsync(int buttonIndex)
    {
        foreach (var button in ButtonList)
        {
            button.IsSelected = button.ButtonIndex == buttonIndex;
            button.UpdateBackgroundColor();
        }
        // Met à jour le contenu en fonction de l'index du bouton sélectionné
        if (buttonIndex > 0 && buttonIndex <= 3)
        {
            SelectedContent = await DataTopicStateAsync(_topicVirtuel);
        }
    }

    public async Task<string> DataTopicStateAsync(string topic)
    {
        var payload = await _mqttConnexion.MqttService.GetMqttMessageAsync(topic);
        return payload ?? "0";
    }

    public int GetNbrBtnActive()
    {
        var BtnActive = ButtonList.Where(button => button.ButtonState == "actif");
        return BtnActive.Count();
    }

    private void AddButton()
    {
        // Logique pour ajouter un nouveau bouton
    }

    private void SwitcherVersHomeassistant(object sender, EventArgs e)
    {
        var _uri = "homeassistant://navigate";
        try
        {
            Launcher.OpenAsync(new Uri(_uri));
        }
        catch (Exception ex) 
        {
            //si l'application n;est pas encore installer
            notification.ShowNotification($"Home assistant n'est pas installer sur votre appareil veuillez l'installer {ex.Message}");
        }
    }

    protected override void OnMqttTopicRecu(string topic, string payload)
    {
        base.OnMqttTopicRecu(topic, payload);
        // Gérer le message MQTT reçu ici
    }
}