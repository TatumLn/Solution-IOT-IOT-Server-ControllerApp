//Initialisation des framework 
const express = require('express');
const bodyParser = require('body-parser');
const WebSocket = require('ws'); 
const ngrok = require('@ngrok/ngrok');
const app = express();
const port = 3000;

// Classe pour stocker les données du simulateur
class SimulatorData {
    constructor() {
        this.temperature = undefined;
        this.humidity = undefined;
        this.luminosite = undefined
        this.climActif = undefined;
        this.deshumidActif = undefined;
        this.lumiereActif = undefined;
        this.arrosoirActif = undefined;
    }
}

const simulatorData = new SimulatorData();
const wss = new WebSocket.Server({ server: app.listen(port) });

//Middleware pour analyser les donnees 
app.use(bodyParser.urlencoded({ extended: true }));

// Route pour recevoir les données du simulateur Wokwi
app.post('/', async (req, res) =>
{
    try {
        await donneesDesCapteurs(req.body);
        res.send('Données du simulateur Wokwi reçues avec succès !');
    } catch (error) {
        console.error('Erreur lors de la réception des données du simulateur Wokwi:', error);
        res.status(500).send('Erreur lors de la réception des données du simulateur Wokwi');
    }
});
async function donneesDesCapteurs(data) {
    return new Promise((resolve, reject) => {
    if (data.type === 'circuit1' ) {
    //Donnees des capteurs du simulateur wokwi
    simulatorData.temperature = data.temperature;
    simulatorData.humidity = data.humidity;
    simulatorData.luminosite = data.luminosite;
    simulatorData.climActif = data.climActif;
    simulatorData.deshumidActif = data.deshumidActif;
    simulatorData.lumiereActif = data.lumiereActif;
    console.log("Données du simulateur Wokwi du circuit 1 reçues -\n" +
                "Température : " + simulatorData.temperature + "°C\n" +
                "Humidité : " + simulatorData.humidity + "%\n" +
                "Luminosité : " + simulatorData.luminosite + "Lux\n" +
                "Climatiseur : " + simulatorData.climActif + "\n" +
                "Deshumidificateur : " + simulatorData.deshumidActif + "\n" +
                "Etat Lumiere : " + simulatorData.lumiereActif);
    } else if (data.type === 'circuit2') {
    simulatorData.arrosoirActif = data.arrosoirActif;
    console.log("Données du simulateur Wokwi du circuit 2 reçues --\n" +
                "Arrosoir : " + simulatorData.arrosoirActif);
    }else {
        // Rejeter la promesse avec un message d'erreur
        reject(new Error('Type de circuit invalide'));
        return; // Arrêter l'exécution de la fonction
    }
    // Envoyer les données mises à jour via WebSocket
    broadcast(JSON.stringify({ temperature: simulatorData.temperature, humidity: simulatorData.humidity, luminosite: simulatorData.luminosite }));

    // Résoudre la promesse une fois le traitement terminé
    resolve();
    });
}

// Gérer les connexions WebSocket
wss.on('connection', function connection(ws) {
    console.log('Connexion au WebSocket depuis IOTController établie');

    // Fonction pour envoyer les données du simulateur au client
    function sendSimulatorData() {
        // Vérifier si les données de température et d'humidité sont définies
        if (simulatorData.temperature !== undefined && simulatorData.humidity !== undefined && simulatorData.luminosite !== undefined) {
            // Si les données sont définies, les envoyer en réponse
            ws.send(JSON.stringify({ temperature: simulatorData.temperature, humidity: simulatorData.humidity, luminosite: simulatorData.luminosite }));
        } else {
            // Si les données ne sont pas définies, envoyer une erreur 404
            ws.send(JSON.stringify({ error: 'Les données du simulateur Wokwi ne sont pas encore reçues.' }));
        }
    }

    // Envoyer les données du simulateur dès qu'une connexion est établie
    sendSimulatorData();

    // Écouter les messages WebSocket du client (optionnel)
ws.on('message', function incoming(message) {
    console.log('Message reçu du client:', message);
});
});

function broadcast(data) {
    wss.clients.forEach(function each(client) {
        if (client.readyState === WebSocket.OPEN) {
            client.send(data);
        }
    });
}
    
console.log('Serveur demarre sur http://localhost:3000');

/*/  Création du tunnel ngrok
ngrok.connect({ addr: port, authtoken: '2dtn2uEgeZfKHEjl8jP8D4Y0EXQ_3MZEtRkqspySJAsj6Uc78' })
  .then(listener => console.log(`Tunnel ngrok créé sur: ${listener.url()}`))
  .catch(err => console.error('Erreur de la creation du tunnel!:', err));*/