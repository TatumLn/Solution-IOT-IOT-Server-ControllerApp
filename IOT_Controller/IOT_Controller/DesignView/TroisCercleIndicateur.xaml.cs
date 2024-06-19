using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;
using IOT_Controller.ViewsModels;
using IOT_Controller.API;

namespace IOT_Controller.DesignView;

public partial class TroisCercleIndicateur : BaseContentView
{

    public ObservableCollection<string> NomTroisIndicateur { get; set; }
    public ObservableCollection<string> DataTroisIndicateur { get; set; }
    //protected readonly CertificatMqtt _certificatMqtt;
    private string[] _topicTroisIndicateur = ["iot/temperature", "iot/luminosite", "iot/humidite"];

    public TroisCercleIndicateur() 
    {
        InitializeComponent();
        DataTroisIndicateur = ["N/A", "N/A", "N/A"];
        NomTroisIndicateur = ["__", "__", "__"];
        this.BindingContext = this;
    }

    public override List<string> GetSubscriptionTopics()
    {
        return [.. _topicTroisIndicateur];
    }

    public override void OnMqttTopicRecu(string topic, string payload)
    {
        base.OnMqttTopicRecu(topic, payload);
        // Gérer le message MQTT reçu ici
        int index = Array.IndexOf(_topicTroisIndicateur, topic);
        if (index >= 0)
        {
            try
            {
                var jsonData = JsonDocument.Parse(payload);
                if ((jsonData.RootElement.TryGetProperty("nom", out var nomElement)) && (jsonData.RootElement.TryGetProperty("valeur", out var valueElement)))
                {
                    DataTroisIndicateur[index] = valueElement.ToString();
                    NomTroisIndicateur[index] = nomElement.ToString();
                }
            }
            catch (Exception ex)
            {
                DataTroisIndicateur[index] = $"{ex.Message}";
                NomTroisIndicateur[index] = $"{ex.Message}";
            }
        }
    }

    private void AfficherChart(object sender, EventArgs e)
    {
        MainViewModel.Instance.IsChartVisible = true;
    }
}