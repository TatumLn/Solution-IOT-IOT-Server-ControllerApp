//Initialisation des framework 
const express = require('express');
const bodyParser = require('body-parser');
const app = express();
const port = 3000;
const { router } = require('./API_REST/api_get');
const { setupWebSocket, clients } = require('./Websocket/websocket');
const { connectLocalClient, connectRemoteClient } = require('./Configuration/configuration');
const { handleMqttData } = require('./MqttHandler/mqttdata');

let isLocalMode = false; // Utiliser le mode local par défaut

// Configurer l'application Express pour utiliser le body-parser
app.use(bodyParser.json());
app.use(bodyParser.urlencoded({ extended: true }));

// Utiliser le routeur API
app.use('/api', router);

// Stocker les données MQTT dans une variable globale
app.set('mqttData', []);

// Configurer WebSocket
setupWebSocket(app);

// Gérer le basculement des modes
app.post('/api/setMode', (req, res) => {
  isLocalMode = req.body.isLocalMode;
  if (isLocalMode) {
    connectLocalClient(handleMqttData, app);
  } else {
    connectRemoteClient(handleMqttData, app);
  }
  res.json({ success: true, isLocalMode });
});

// Initialiser en mode local
connectRemoteClient(handleMqttData, app);
    
console.log(`Serveur demarre sur http://localhost:${port}`);
