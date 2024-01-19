using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using HamerSoft.PuniTY.Configuration;

namespace HamerSoft.PuniTY.Tests.Editor
{
    public class MockClient : IPunityClient
    {
        private readonly MockServer _server;
        private Stream _stream;
        private ClientArguments _startArguments;
        private bool _exitted;

        public Guid Id { get; }
        public bool IsConnected => _stream != null;
        public bool HasExited => _exitted;
        public event Action<string> ResponseReceived;
        public event Action Exited;

        internal MockClient(Guid id, MockServer server)
        {
            _server = server;
            Id = id;
        }

        public async Task Write(string text)
        {
            var bytes = System.Text.Encoding.ASCII.GetBytes(text);
            await _stream.WriteAsync(bytes, 0, bytes.Length);
        }

        public Task WriteLine(string text)
        {
            throw new NotImplementedException();
        }

        public Task Write(byte[] bytes)
        {
            throw new NotImplementedException();
        }

        public void Start(ClientArguments startArguments)
        {
            _startArguments = startArguments;

            _server.ConnectClient(Id, this);
            var idBytes = System.Text.Encoding.ASCII.GetBytes(Id.ToString());
            _stream.Write(idBytes, 0, idBytes.Length);
        }

        public async Task<bool> StartAsync(ClientArguments startArguments, CancellationToken token = default)
        {
            Start(startArguments);
            await Task.Delay(100, token);
            return true;
        }

        public void Stop()
        {
            _exitted = true;
            CloseStream(_stream);
            _stream = null;
        }

        private void CloseStream(Stream stream)
        {
            stream?.Dispose();
            stream?.Close();
        }

        public void Connect(Stream stream)
        {
            _stream = stream;
        }
    }
}