using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using HamerSoft.PuniTY.Configuration;

namespace HamerSoft.PuniTY.Tests.Editor
{
    internal class MockTCPClient : IPunityClient
    {
        private Stream _stream;
        private ClientArguments _startArguments;
        private bool _exitted;
        private TcpClient _tcpClient;
        private NetworkStream _ioStream;

        public Guid Id { get; }
        public bool IsConnected => _stream != null;
        public bool HasExited => _exitted;
        public event Action<string> ResponseReceived;
        public event Action<byte[]> BytesReceived;
        public event Action Exited;

        public MockTCPClient(Guid id)
        {
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
            _tcpClient = new TcpClient(startArguments.Ip.ToString(), (int)startArguments.Port);
            _ioStream = _tcpClient.GetStream();
            var idBytes = System.Text.Encoding.ASCII.GetBytes(Id.ToString());
            _ioStream.Write(idBytes, 0, idBytes.Length);
            Connect(_ioStream);
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
            CloseStream(_ioStream);
            _stream = null;
            _ioStream = null;
            _tcpClient?.Close();
            _tcpClient?.Dispose();
            _tcpClient = null;
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