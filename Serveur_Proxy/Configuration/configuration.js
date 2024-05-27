const mqtt = require('mqtt');

let mqttClient = null;

//Control en local
const connectLocalClient = (handleMqttData, app) => {
  if (mqttClient) {
    mqttClient.end();
  }

  mqttClient = mqtt.connect('mqtt://localhost:1883', {
    clientId: 'NodeJSClientLocal'
  });

  mqttClient.on('connect', () => {
    console.log('Connection a votre HiveMQ broker MQTT local reussi!');
    mqttClient.subscribe('iot/solution', (err) => {
      if (!err) {
        console.log('Souscription au topic: iot/solution réussie!');
      } else {
        console.error('Erreur à la souscription au topic:', err);
      }
    });
  });

  mqttClient.on('message', (topic, message) => {
    handleMqttData(topic, message, 'local');
  });
};

//Control distance (Remote)
const connectRemoteClient = (handleMqttData, app) => {
  if (mqttClient) {
    mqttClient.end();
  }

  mqttClient = mqtt.connect('mqtts://votreidcloud.hivemq.cloud', {
    clientId: 'NodeJSClientRemote',
    username: 'VotreUsername',
    password: 'VotreMotdePasse',
    port: 8883
  });

  mqttClient.on('connect', () => {
    console.log('Connection à votre Cluster HiveMQ Cloud reussi!');
    mqttClient.subscribe('iot/solution', (err) => {
      if (!err) {
        console.log('Souscription au topic: iot/solution réussie!');
      } else {
        console.error('Erreur à la souscription au topic:', err);
      }
    });
  });

  mqttClient.on('message', (topic, message) => {
    handleMqttData(topic, message, 'remote');
  });
};

module.exports = { connectLocalClient, connectRemoteClient };
