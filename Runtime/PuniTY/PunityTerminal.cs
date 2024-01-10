using System;
using HamerSoft.PuniTY.Core.Client;
using HamerSoft.PuniTY.Core.Logging;

namespace HamerSoft.PuniTY
{
    public class PunityTerminal : IPunityTerminal
    {
        private readonly ILogger _logger;
        private PunityServer _server;
        private IPunityClient _client;
        private ITerminalUI _ui;

        public event Action Stopped;
        public bool IsRunning { get; private set; }


        public PunityTerminal(ILogger logger = null)
        {
            _logger = logger ?? new EditorLogger();
        }

        public void Start(StartArguments arguments, ITerminalUI ui)
        {
            if (IsRunning)
            {
                _logger.LogWarning("Terminal already running");
                return;
            }

            _ui = ui;
            _server = new PunityServer(_logger);
            _server.ResponseReceived += ServerResponseReceived;
            _server.ConnectionLost += ServerOnConnectionLost;
            _server.Start(arguments);
            _client = new PunityClient(_logger);
            _client.Exited += ClientExited;
            _client.Start(arguments);
            if (ui != null)
            {
                _ui.Written += UiWritten;
                _ui.WrittenLine += UiWrittenLine;
                _ui.WrittenByte += UiWrittenByte;
            }

            IsRunning = true;
        }

        private void ServerOnConnectionLost()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            if (_client != null)
            {
                _client.Exited -= ClientExited;
                _client.Stop();
                _client = null;
            }

            if (_server != null)
            {
                _server.ResponseReceived -= ServerResponseReceived;
                _server.ConnectionLost -= ServerOnConnectionLost;
                _server.Stop();
                _server = null;
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
            await _server.WriteLine(text);
        }

        private async void UiWrittenByte(byte[] bytes)
        {
            await _server.Write(bytes);
        }

        private async void UiWritten(string text)
        {
            await _server.Write(text);
        }

        private void ServerResponseReceived(string message)
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