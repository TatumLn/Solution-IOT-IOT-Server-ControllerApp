
const { filterData } = require('../API_REST/api_filtreData');

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

    // Filtrer les données
    const filteredData = filterData(jsonData);

    // Afficher les données après filtrage
    console.log('Données après filtrage:', filteredData);

    // Publier les données filtrées sur des topics portant leurs noms
    const mqttClient = app.get('mqttClient');
    for (const key in filteredData) {
      if (filteredData.hasOwnProperty(key)) {
        const topicName = `iot/${key}`;
        const payload = JSON.stringify({ [key]: filteredData[key] });
        mqttClient.publish(topicName, payload, (err) => {
          if (err) {
            console.error(`Erreur lors de la publication sur le topic ${topicName}:`, err);
          } else {
            console.log(`Donnée publiée sur ${topicName}:`, payload);
          }
        });
                                            }
                                      }

  } catch (error) {
    console.error('Erreur lors du parsing du JSON:', error);
    console.error('Message incorrect:', messageString);
  }
}

module.exports = { handleMqttData };
