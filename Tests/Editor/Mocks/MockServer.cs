using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using HamerSoft.PuniTY.Configuration;
using HamerSoft.PuniTY.Logging;

namespace HamerSoft.PuniTY.Tests.Editor
{
    internal class MockServer : IPunityServer
    {
        public event Action<Guid> ConnectionLost;
        public event Action<Guid, Stream> ClientConnected;
        public event Action Stopped;

        public bool IsConnected { get; }
        public int ConnectedClients { get; }
        private Dictionary<Guid, Stream> _clients;
        private readonly ILogger _logger;

        public MockServer(ILogger logger)
        {
            _clients = new();
            _logger = logger;
        }

        public void Start(StartArguments startArguments)
        {
        }

        public void ConnectClient(Guid id, MockClient client)
        {
            var stream = new MemoryStream();
            _clients.Add(id, stream);
            ClientConnected?.Invoke(id, stream);
            client.Connect(stream);
        }

        public void Stop(ref IPunityClient client)
        {
            if (!_clients.Remove(client.Id, out var stream))
                return;
            stream.Dispose();
            stream.Close();
            client.Stop();
            ConnectionLost?.Invoke(client.Id);
            client = null;
        }

        public void Stop()
        {
            foreach (var client in _clients)
            {
                client.Value.Dispose();
                client.Value.Close();
                ConnectionLost?.Invoke(client.Key);
            }

            _clients.Clear();
            Stopped?.Invoke();
        }
    }
}