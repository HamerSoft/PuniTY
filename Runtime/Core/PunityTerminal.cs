using System;
using System.IO;

namespace HamerSoft.PuniTY.Core
{
    internal class PunityTerminal : IPunityTerminal
    {
        private readonly IPunityServer _server;
        private readonly ILogger _logger;
        private IPunityClient _client;
        private ITerminalUI _ui;

        public event Action Stopped;
        public bool IsRunning { get; private set; }


        internal PunityTerminal(IPunityServer server, IPunityClient client, ILogger logger)
        {
            _logger = logger;
            _server = server;
            _client = client;
        }

        public void Start(StartArguments arguments, ITerminalUI ui)
        {
            if (IsRunning)
            {
                _logger.LogWarning("Terminal already running");
                return;
            }

            _ui = ui;

            _server.ConnectionLost += ServerOnConnectionLost;
            _server.ClientConnected += ServerOnClientConnected;
            _client.Exited += ClientExited;
            _client.ResponseReceived += ClientResponseReceived;
            _client.Start(arguments);
            if (ui != null)
            {
                _ui.Written += UiWritten;
                _ui.WrittenLine += UiWrittenLine;
                _ui.WrittenByte += UiWrittenByte;
            }

            IsRunning = true;
        }

        private void ServerOnClientConnected(Guid id, Stream stream)
        {
            _client.Connect(id, stream);
        }

        public void Stop()
        {
            if (_server != null)
            {
                _server.Stop(_client);
                _server.ConnectionLost -= ServerOnConnectionLost;
            }

            if (_client != null)
            {
                _client.Exited -= ClientExited;
                _client.ResponseReceived -= ClientResponseReceived;
                _client.Stop();
                _client = null;
            }

            if (_ui != null)
            {
                _ui.Written -= UiWritten;
                _ui.WrittenLine -= UiWrittenLine;
                _ui.WrittenByte -= UiWrittenByte;
            }

            Stopped?.Invoke();
            IsRunning = false;
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
            throw new NotImplementedException("Not sure when client disconnects from server!");
        }

        private void ClientResponseReceived(string message)
        {
            _ui.Print(message);
        }

        private void ClientExited()
        {
            _ui.Print("Connection to client lost! Exiting...");
            Stop();
        }

        public void Dispose()
        {
            Stop();
        }
    }
}