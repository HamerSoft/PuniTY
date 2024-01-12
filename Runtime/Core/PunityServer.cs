using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace HamerSoft.PuniTY.Core
{
    // make server into hub to accept clients
    // pass the stream along to create a client
    // write through the client instead of the server
    // write tests based on the stream abstraction
    internal class PunityServer : IPunityServer
    {
        public event Action<Guid> ConnectionLost;
        public event Action<Guid, Stream> ClientConnected;

        public bool IsConnected => _clients.Count > 0;
        public int ConnectedClients => _clients.Count;

        private readonly ILogger _logger;
        private Thread _listeningThread;
        private TcpListener _server;

        private StartArguments _startArguments;
        private bool _stopped;

        private Dictionary<Guid, TcpClient> _clients;

        internal PunityServer(ILogger logger)
        {
            _clients = new();
            _logger = logger;
        }

        public void Start(StartArguments startArguments)
        {
            _stopped = false;
            _startArguments = startArguments;
            if (!_startArguments.IsValid(out var message))
                throw new ArgumentException(message);

            try
            {
                _server = new TcpListener(_startArguments.Ip, (int)_startArguments.Port);
                _server.Start();
                _listeningThread = new Thread(WaitForClient);
                _listeningThread?.Start();
            }
            catch (SocketException e)
            {
                _logger.LogError("SocketException:", e);
            }
        }

        public void Stop(IPunityClient client)
        {
            if (client == null || !_clients.Remove(client.Id, out var tcpClient))
                return;
            tcpClient.Close();
            tcpClient.Dispose();
            OnConnectionLost(client.Id);
        }

        private void WaitForClient()
        {
            while (!_stopped)
            {
                _logger.Log("Waiting for a connection... ");
                var tcpClient = _server.AcceptTcpClient();
                if (tcpClient != null)
                {
                    var id = Guid.NewGuid();
                    _clients[id] = tcpClient;
                    _logger.Log("Established Connection with new client!");
                    ClientConnected?.Invoke(id, tcpClient.GetStream());
                }
            }
        }

        public void Stop()
        {
            _stopped = true;
            _logger.Log("Server stopping!");
            _listeningThread?.Abort();
            _listeningThread = null;
            _server?.Stop();
            foreach (var client in _clients)
            {
                client.Value?.Close();
                client.Value?.Dispose();
                OnConnectionLost(client.Key);
            }

            _clients.Clear();
            _server = null;
        }

        private void OnConnectionLost(Guid id)
        {
            ConnectionLost?.Invoke(id);
        }

        public void Dispose()
        {
            Stop();
        }
    }
}