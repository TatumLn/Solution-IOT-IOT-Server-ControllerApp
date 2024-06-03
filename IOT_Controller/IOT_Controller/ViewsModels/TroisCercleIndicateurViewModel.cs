using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOT_Controller.ViewsModels
{
    public class TroisCercleIndicateurViewModel
    {
        public ObservableCollection<string> DataTroisIndicateur { get; set; }
        //protected readonly CertificatMqtt _certificatMqtt;
        public string[] _topicTroisIndicateur = ["iot/temperature", "iot/luminosite", "iot/humidite"];

        public TroisCercleIndicateurViewModel()
        {
            DataTroisIndicateur =
            [
                "N/A", //Temperature
                "N/A", //Luminosite
                "N/A"  //Humidite
            ];
        }

        private void OnMqttTopicRecu(string topic, string payload)
        {
            int index = Array.IndexOf(_topicTroisIndicateur, topic);
            if (index >= 0)
            {
                DataTroisIndicateur[index] = payload;
            }
        }
    }
}
