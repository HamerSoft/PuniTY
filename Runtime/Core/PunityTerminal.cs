using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using HamerSoft.PuniTY.Configuration;
using HamerSoft.PuniTY.Logging;

namespace HamerSoft.PuniTY.Core
{
    internal class PunityTerminal : IPunityTerminal
    {
        private readonly IPunityServer _server;
        private readonly ILogger _logger;
        private IPunityClient _client;
        private ITerminalUI _ui;

        public event Action Stopped;
        public event Action<string> ResponseReceived;
        public event Action<byte[]> BytesReceived;
        public bool IsRunning { get; private set; }

        internal PunityTerminal(IPunityServer server, IPunityClient client, ILogger logger)
        {
            _server = server;
            _client = client;
            _logger = logger;
        }

        public void Start(ClientArguments arguments, ITerminalUI ui)
        {
            if (IsRunning)
            {
                _logger.LogWarning("Terminal already running");
                return;
            }

            IsRunning = true;
            AssignEvents(ui);
            _client.Start(arguments);
        }

        public async Task StartAsync(ClientArguments arguments, ITerminalUI ui = null,
            CancellationToken token = default)
        {
            if (IsRunning)
            {
                _logger.LogWarning("Terminal already running");
                return;
            }

            IsRunning = true;
            AssignEvents(ui);
            await _client.StartAsync(arguments, token);
        }

        private void AssignEvents(ITerminalUI ui)
        {
            _ui = ui;
            _server.ConnectionLost += ServerOnConnectionLost;
            _server.ClientConnected += ServerOnClientConnected;
            _client.Exited += ClientExited;
            _client.ResponseReceived += ClientResponseReceived;
            _client.BytesReceived += ClientBytesReceived;
            if (ui != null)
            {
                _ui.Closed += Stop;
                _ui.Written += UiWritten;
                _ui.WrittenLine += UiWrittenLine;
                _ui.WrittenByte += UiWrittenByte;
            }
        }

        private void ServerOnClientConnected(Guid id, Stream stream)
        {
            if (_client.Id == id)
                _client.Connect(stream);
        }

        public void Stop()
        {
            if (_server != null)
            {
                _server.ConnectionLost -= ServerOnConnectionLost;
                _server.ClientConnected -= ServerOnClientConnected;
            }

            if (_client != null)
            {
                _client.Exited -= ClientExited;
                _client.ResponseReceived -= ClientResponseReceived;
                _client.BytesReceived -= ClientBytesReceived;
                _client.Stop();
                _client = null;
            }

            if (_ui != null)
            {
                _ui.Closed -= Stop;
                _ui.Written -= UiWritten;
                _ui.WrittenLine -= UiWrittenLine;
                _ui.WrittenByte -= UiWrittenByte;
            }

            Stopped?.Invoke();
            IsRunning = false;
        }

        public async Task Write(string text)
        {
            if (_client != null)
                await _client.Write(text);
        }

        public async Task WriteLine(string text)
        {
            if (_client != null)
                await _client.WriteLine(text);
        }

        public async Task Write(byte[] bytes)
        {
            if (_client != null)
                await _client.Write(bytes);
        }

        private async void UiWrittenLine(string text)
        {
            await _client.WriteLine(text);
        }

        private async void UiWrittenByte(byte[] bytes)
        {
            await _client.Write(bytes);
        }

        private async void UiWritten(string text)
        {
            await _client.Write(text);
        }

        private void ServerOnConnectionLost(Guid guid)
        {
            if (guid == _client.Id)
                Stop();
        }

        private void ClientResponseReceived(string message)
        {
            ResponseReceived?.Invoke(message);
            _ui?.Print(message);
        }

        private void ClientBytesReceived(byte[] message)
        {
            BytesReceived?.Invoke(message);
            _ui?.Print(message);
        }

        private void ClientExited()
        {
            _ui?.Print("Connection to client lost! Exiting...");
            Stop();
        }
    }
}