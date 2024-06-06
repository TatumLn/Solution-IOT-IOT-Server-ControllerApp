
const { filterData } = require('../API_REST/api_filtreData');

let oldData = null;

function handleMqttData(topic, message, source, app) {
  const messageString = message.toString();

  try {
    const jsonData = JSON.parse(messageString);

    // Afficher les données du broker  en JSON
    console.log('Données du broker en JSON:', jsonData);

    // Ajouter le message au tableau `mqttData`
    const mqttData = app.get('mqttData');
    mqttData.push(jsonData);
    app.set('mqttData', mqttData);

     // Comparer les nouvelles données avec les anciennes
     if (!oldData) {
      // Si c'est la première réception, publier toutes les valeurs
      publishValeur(jsonData, app);
      // Mettre à jour les anciennes valeurs
      oldData = jsonData;
    } else {
      // Vérifier chaque nouvelle donnée pour les changements
      Object.keys(jsonData).forEach((key) => {
        if (oldData[key] !== jsonData[key]) {
          // Si la valeur a changé, la publier
          publishValeurMAJ(key, jsonData[key], app);
          // Mettre à jour les anciennes valeurs
          oldData[key] = jsonData[key];
        }
      });
    }
  } catch (error) {
    console.error('Erreur lors du parsing du JSON:', error);
    console.error('Message incorrect:', messageString);
  }
}

function publishValeur(jsonData, app) {
  // Filtrer les données si nécessaire
  const filteredData = filterData(jsonData);

  // Afficher les données après filtrage
  console.log('Données après filtrage:', filteredData);

  // Publier les données filtrées sur des topics portant leurs noms
  const mqttClient = app.get('mqttClient');
  filteredData.forEach(({ nom, valeur }) => {
    const topicName = `iot/${nom}`;
    const payload = JSON.stringify({nom, valeur});
    mqttClient.publish(topicName, payload, (err) => {
      if (err) {
        console.error(`Erreur lors de la publication sur le topic ${topicName}:`, err);
      } else {
        console.log(`Donnée publiée sur ${topicName}:`, payload);
      }
    });
  });
}

function publishValeurMAJ(nom, valeur, app) {
  // Publier uniquement la valeur qui a changé
  const mqttClient = app.get('mqttClient');
  const topicName = `iot/${nom}`;
  const payload = JSON.stringify({ nom, valeur });
  mqttClient.publish(topicName, payload, (err) => {
    if (err) {
      console.error(`Erreur lors de la publication sur le topic ${topicName}:`, err);
    } else {
      console.log(`Donnée publiée sur ${topicName}:`, payload);
    }
  });
}

module.exports = { handleMqttData };
