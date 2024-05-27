const WebSocket = require('ws');
const clients = [];

const setupWebSocket = (server) => {
  const wss = new WebSocket.Server({ server });
  wss.on('connection', (ws) => {
    clients.push(ws);
    ws.on('close', () => {
      clients.splice(clients.indexOf(ws), 1);
    });
  });

  server.on('filteredData', (filteredData) => {
    clients.forEach((client) => {
      client.send(JSON.stringify(filteredData));
    });
  });
};

module.exports = { setupWebSocket, clients };
