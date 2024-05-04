using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace IOT_Controller.API
{
    public class CommunicationService
    {
        private readonly ClientWebSocket _webSocket;
        private CancellationTokenSource _cancellationTokenSource;
        public bool IsConnected { get; private set; }
        public event EventHandler<DataReceivedEventArgs>? DataReceived;
        private string? _errorMessage;
        private string? _connectingMessage;

        public CommunicationService()
        {
            _webSocket = new ClientWebSocket();
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public string? ConnectingMessage
        {
            get { return _connectingMessage; }
            private set
            {
                _connectingMessage = value;
            }
        }

        public string? ErrorMessage
        {
            get { return _errorMessage; }
            private set
            {
                _errorMessage = value;
            }
        }

        public async Task ConnectWebSocket(Uri uri)
        {
            try
            {
                await _webSocket.ConnectAsync(uri, CancellationToken.None);
                ConnectingMessage = "Connexion au serveur réussi!";
                IsConnected = true;
                // Commencer à recevoir les données en boucle
                await ReceiveLoop();
            }
            catch (Exception ex)
            {
                ErrorMessage = "Erreur de connexion au WebSocket : " + ex.Message;
                IsConnected = false;
            }
        }

        private async Task ReceiveLoop()
        {
            byte[] buffer = new byte[1024];
            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                try
                {
                    WebSocketReceiveResult result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        OnDataReceived(message);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erreur de réception WebSocket : " + ex.Message);
                }
            }
        }

        private void OnDataReceived(string data)
        {
            DataReceived?.Invoke(this, new DataReceivedEventArgs(data));
        }

        public async Task DisconnectWebSocket()
        {
            _cancellationTokenSource.Cancel();
            await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Fermeture", CancellationToken.None);
        }
    }

    public class DataReceivedEventArgs : EventArgs
    {
        public string Data { get; }

        public DataReceivedEventArgs(string data)
        {
            Data = data;
        }
    }
}
