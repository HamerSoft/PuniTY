﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using HamerSoft.PuniTY.Configuration;
using HamerSoft.PuniTY.Logging;

namespace HamerSoft.PuniTY.Core
{
    internal class PunityServer : IPunityServer
    {
        public event Action<Guid> ConnectionLost;
        public event Action<Guid, Stream> ClientConnected;
        public event Action Stopped;

        public bool IsConnected => _clients.Count > 0;
        public int ConnectedClients => _clients.Count;

        private readonly ILogger _logger;
        private Thread _listeningThread;
        private TcpListener _tcpServer;

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
            string message = null;
            if (_startArguments == null || !_startArguments.IsValid(out message))
                throw new ArgumentException(message ?? "Cannot start server with NULL arguments");

            try
            {
                _tcpServer = new TcpListener(_startArguments.Ip, (int)_startArguments.Port);
                _tcpServer.Start();
                _listeningThread = new Thread(WaitForClient);
                _listeningThread?.Start();
            }
            catch (SocketException e)
            {
                _logger.LogError("SocketException:", e);
            }
        }

        public void Stop(ref IPunityClient client)
        {
            if (client == null || !_clients.Remove(client.Id, out var tcpClient))
                return;
            tcpClient.Close();
            tcpClient.Dispose();
            client.Stop();
            OnConnectionLost(client.Id);
            client = null;
            Stopped?.Invoke();
        }

        private void WaitForClient()
        {
            while (!_stopped)
            {
                _logger.Log("Waiting for a connection... ");
                byte[] buffer = new byte[36];
                var tcpClient = _tcpServer.AcceptTcpClient();
                if (tcpClient != null)
                {
                    _ = tcpClient.GetStream().Read(buffer, 0, buffer.Length);
                    _logger.Log("Established Connection with new client! Awaiting confirmation...");
                    var responseID = System.Text.Encoding.UTF8.GetString(buffer);
                    if (!Guid.TryParse(responseID, out var id))
                    {
                        _logger.LogWarning($"Client connected but invalid ID: {responseID}!");
                        continue;
                    }

                    _clients[id] = tcpClient;
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
            _tcpServer?.Stop();
            foreach (var client in _clients)
            {
                client.Value?.Close();
                client.Value?.Dispose();
                OnConnectionLost(client.Key);
            }

            _clients.Clear();
            _tcpServer = null;
            Stopped?.Invoke();
        }

        private void OnConnectionLost(Guid id)
        {
            ConnectionLost?.Invoke(id);
        }
    }
}