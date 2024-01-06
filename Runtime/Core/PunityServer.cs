using System;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using HamerSoft.PuniTY.Core.Logging;

namespace HamerSoft.PuniTY.TCP.Core
{
    public class PunityServer : IPunityServer
    {
        public event Action<string> ResponseReceived;

        public bool IsConnected => _client?.Connected == true;

        private readonly UTF8Encoding _encoding;
        private readonly Regex _ansiRegex;
        private readonly ILogger _logger;
        private Thread _listeningThread;
        private TcpListener _server;
        private TcpClient _client;
        private NetworkStream _ioStream;
        private StartArguments _startArguments;

        internal PunityServer(ILogger logger = null)
        {
            _logger = logger ?? new EditorLogger();
            _encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
            _ansiRegex = new Regex(
                @"[\u001B\u009B][[\]()#;?]*(?:(?:(?:[a-zA-Z\d]*(?:;[a-zA-Z\d]*)*)?\u0007)|(?:(?:\d{1,4}(?:;\d{0,4})*)?[\dA-PRZcf-ntqry=><~]))");
        }

        public void Start(StartArguments args)
        {
            _startArguments = args;
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

        private void WaitForClient()
        {
            _logger.Log("Waiting for a connection... ");
            _client = _server.AcceptTcpClient();
            _logger.Log("Connected!");
            _ioStream = _client.GetStream();
            StartReading();
        }

        private void StartReading()
        {
            const int readSize = 4096;
            byte[] buffer = new byte[readSize];
            AsyncCallback callback = null;
            callback = ar =>
            {
                int bytesRead = _ioStream.EndRead(ar);
                if (bytesRead < 0)
                    return;

                var output = _encoding.GetString(buffer, 0, bytesRead);
                output = _ansiRegex.Replace(output, string.Empty);

                OnResponseReceived(output);

                Array.Clear(buffer, 0, buffer.Length);
                _ioStream.BeginRead(buffer, 0, readSize, callback, this);
            };

            _ioStream.BeginRead(buffer, 0, readSize, callback, this);
        }

        private void OnResponseReceived(string response)
        {
            ResponseReceived?.Invoke(response);
        }

        public void Stop()
        {
            _listeningThread?.Abort();
            _listeningThread = null;
            _server?.Stop();
            _server = null;
            _ioStream?.Close();
            _ioStream?.Dispose();
            _ioStream = null;
        }

        public async Task Write(string text)
        {
            var bytes = GetBytes(text);
            await _ioStream.WriteAsync(bytes, 0, bytes.Length);
        }

        public async Task WriteLine(string text)
        {
            var bytes = GetBytes($"{Environment.NewLine}{text}");
            await _ioStream.WriteAsync(bytes, 0, bytes.Length);
        }

        public async Task Write(byte[] bytes)
        {
            await _ioStream.WriteAsync(bytes, 0, bytes.Length);
        }
        
        public void Dispose()
        {
           Stop();
        }
        
        private static byte[] GetBytes(string message)
        {
            return Encoding.ASCII.GetBytes(message);
        }
    }
}