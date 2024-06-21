// Fonction de filtrage des données
const filterData = (data) => {
  const filteredData = [];
  Object.keys(data).forEach(key => {
    let valeur = data[key];
    // Ajouter les unités de mesure où nécessaire
    if (key === 'temperature') {
      valeur = `${valeur}°C`;
    } else if (key === 'humidite') {
      valeur = `${valeur}%`;
    } else if (key === 'luminosite') {
      valeur = `${valeur}lx`;
    }
    filteredData.push({nom: key, valeur: valeur });
  });
  return filteredData;
};

module.exports = { filterData };
