const { filterData } = require('../API_REST/api_filtreData');

function handleMqttData(topic, message, source, app) {
  const messageString = message.toString();
  console.log(`Message brut reçu de ${source} (${topic}): ${messageString}`);

  try {
    const jsonData = JSON.parse(messageString);

    // Afficher les données du cluster en JSON
    console.log('Données du cluster en JSON:', jsonData);

    // Ajouter le message au tableau `mqttData`
    const mqttData = app.get('mqttData');
    mqttData.push(jsonData);
    app.set('mqttData', mqttData);

    // Filtrer les données
    const filteredData = filterData(jsonData);

    // Afficher les données après filtrage
    console.log('Données après filtrage:', filteredData);

    // Émettre les données filtrées via WebSocket
    filteredData.forEach(data => {
      app.emit('filteredData', data);
    });
  } catch (error) {
    console.error('Erreur lors du parsing du JSON:', error);
    console.error('Message incorrect:', messageString);
  }
}

module.exports = { handleMqttData };
