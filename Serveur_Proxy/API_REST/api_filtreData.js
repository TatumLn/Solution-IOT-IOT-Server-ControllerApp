// Fonction de filtrage des données
const filterData = (data) => {
  const filteredData = [];
  Object.keys(data).forEach(key => {
    filteredData.push({nom: key, valeur: data[key] });
  });
  return filteredData;
};

module.exports = { filterData };
