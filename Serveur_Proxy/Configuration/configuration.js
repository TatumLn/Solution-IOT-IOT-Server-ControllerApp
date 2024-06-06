const mqtt = require('mqtt');
//const fs = require('fs');
//const path = require('path');

let mqttClient = null;

//Control en local
const connectLocalClient = (handleMqttData, app) => {
  if (mqttClient) {
    mqttClient.end();
  }

  // Chemins vers les fichiers de certificats
  //const KEY = fs.readFileSync(path.join(__dirname, '../Certificat/mqtt-client-key.pem'));
  //const CERT = fs.readFileSync(path.join(__dirname, '../Certificat/mqtt-client-cert.pem'));
  //const CA = fs.readFileSync(path.join(__dirname, '../Certificat/hivemq-server-cert.pem'));

  mqttClient = mqtt.connect('mqtt://192.168.0.10', {
    clientId: 'NodeJSClientLocal',
    username: 'admin-user',
    password: 'admin-password',
    //key: KEY,
    //cert: CERT,
    //ca: CA,
    //rejectUnauthorized: true,
    port: 1883
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

  mqttClient = mqtt.connect('mqtts://prosper-dafq7h.a01.euc1.aws.hivemq.cloud', {
    clientId: 'NodeJSClientRemote',
    username: 'ProsperCluster',
    password: 'Jqb?5PCHeF7S4*Gr9m.$p',
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
    handleMqttData(topic, message, 'remote', app);
  });

    // Stocker le client MQTT dans `app`
    app.set('mqttClient', mqttClient);
};

module.exports = { connectLocalClient, connectRemoteClient };
