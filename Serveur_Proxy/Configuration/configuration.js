const mqtt = require('mqtt');
const fs = require('fs');
const path = require('path');

let mqttClient = null;

//Control en local
const connectLocalClient = (handleMqttData, app) => {
  if (mqttClient) {
    mqttClient.end();
  }

  // Chemins vers les fichiers de certificats
  const KEY = fs.readFileSync(path.join(__dirname, '../Certificat/<MettreIciVotreKeyClient.pem>'));
  const CERT = fs.readFileSync(path.join(__dirname, '../Certificat/<MettreIciVotreCertClient.pem>'));
  const CA = fs.readFileSync(path.join(__dirname, '../Certificat/<MettreIciVotreCertCA.pem>'));

  mqttClient = mqtt.connect('mqtt://localhost', {
    clientId: 'NodeJSClientLocal',
    username: 'VotreUserName',
    password: 'VotreMotDePasse',
    key: KEY,
    cert: CERT,
    ca: CA,
    rejectUnauthorized: true,
    port: 8883
  });

  mqttClient.on('connect', () => {
    console.log('Connection a votre HiveMQ broker MQTT local reussi!');
    mqttClient.subscribe('<NomdeVotreTopic>', (err) => {
      if (!err) {
        console.log('Souscription au topic: <NomVotreTopic> réussie!');
      } else {
        console.error('Erreur à la souscription au topic:', err);
      }
    });
  });

  mqttClient.on('message', (topic, message) => {
    handleMqttData(topic, message, 'local', app);
  });

    // Stocker le client MQTT dans `app`
    app.set('mqttClient', mqttClient);
};

//Control distance (Remote)
const connectRemoteClient = (handleMqttData, app) => {
  if (mqttClient) {
    mqttClient.end();
  }

  mqttClient = mqtt.connect('mqtts://<AdressdeVotreHiveMQCloud>', {
    clientId: 'NodeJSClientRemote',
    username: 'VotreUserName',
    password: 'VotreMotdePasse',
    port: 8883
  });

  mqttClient.on('connect', () => {
    console.log('Connection à votre Cluster HiveMQ Cloud reussi!');
    mqttClient.subscribe('<NomdeVotreTopic>', (err) => {
      if (!err) {
        console.log('Souscription au topic: <NomdeVotreTopic> réussie!');
      } else {
        console.error('Erreur à la souscription au topic:', err);
      }
    });
  });

  mqttClient.on('message', (topic, message) => {
    handleMqttData(topic, message, 'remote', app);
  });

    // Stocker le client MQTT dans `app`
    app.set('mqttClient', mqttClient);
};

module.exports = { connectLocalClient, connectRemoteClient };
