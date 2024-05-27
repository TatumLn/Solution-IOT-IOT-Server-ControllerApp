// Fonction de filtrage des données
const filterData = (data) => {
  const filteredData = [];
  Object.keys(data).forEach(key => {
    filteredData.push({ [key]: data[key] });
  });
  return filteredData;
};

module.exports = { filterData };
