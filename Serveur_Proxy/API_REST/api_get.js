const express = require('express');
const router = express.Router();
const { filterData } = require('./api_filtreData');

// Par défaut, le mode est local
let isLocalMode = true; 

// Route pour obtenir les données filtrées
router.get('/filter', (req, res) => {
  const data = req.app.get('mqttData');
  const filteredData = filterData(data);
  res.json(filteredData);
});

// Route pour basculer entre les modes
router.post('/setMode', (req, res) => {
  isLocalMode = req.body.isLocalMode;
  res.json({ success: true, isLocalMode });
});

router.get('/getMode', (req, res) => {
  res.json({ isLocalMode });
});

module.exports = { router, isLocalMode };
