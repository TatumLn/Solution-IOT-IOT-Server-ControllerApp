//Initialisation des framework 
const express = require('express');
const bodyParser = require('body-parser');
const WebSocket = require('ws'); 
const ngrok = require('@ngrok/ngrok');
const mqtt = require('mqtt');
const app = express();
const port = 3000;

// Classe pour stocker les données du simulateur
class SimulatorData {
    constructor() {
        this.temperature = undefined;
        this.humidity = undefined;
    }
}

const simulatorData = new SimulatorData();
const wss = new WebSocket.Server({ server: app.listen(port) });

//Middleware pour analyser les donnees 
app.use(bodyParser.urlencoded({ extended: true }));

// Route pour recevoir les données du simulateur Wokwi
app.post('/', (req, res) =>
{
    simulatorData.temperature = req.body.temperature;
    simulatorData.humidity = req.body.humidity;
    console.log('Données du simulateur Wokwi reçues - Température :', simulatorData.temperature, '°C, Humidité :', simulatorData.humidity, '%');
    res.send('Données du simulateur Wokwi reçues avec succès !');

    // Envoyer les données mises à jour via WebSocket
    broadcast(JSON.stringify({ temperature: simulatorData.temperature, humidity: simulatorData.humidity }));
});

// Gérer les connexions WebSocket
wss.on('connection', function connection(ws) {
    console.log('Nouvelle connexion WebSocket établie');

    // Fonction pour envoyer les données du simulateur au client
    function sendSimulatorData() {
        // Vérifier si les données de température et d'humidité sont définies
        if (simulatorData.temperature !== undefined && simulatorData.humidity !== undefined) {
            // Si les données sont définies, les envoyer en réponse
            ws.send(JSON.stringify({ temperature: simulatorData.temperature, humidity: simulatorData.humidity }));
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