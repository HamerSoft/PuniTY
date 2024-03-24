using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using HamerSoft.PuniTY.Configuration;
using UnityEngine;
using ILogger = HamerSoft.PuniTY.Logging.ILogger;

namespace HamerSoft.PuniTY.Core
{
    internal class PunityClient : IPunityClient
    {
        private Process _myProcess;
        private ClientArguments _startArguments;
        private Stream _stream;
        private readonly ILogger _logger;
        private AsyncCallback _callback;
        private bool _isStarted;

        public Guid Id { get; private set; }
        public bool IsConnected => _stream != null;
        public bool HasExited { get; protected set; }
        public event Action<byte[]> BytesReceived;
        public event Action Exited;
        public event Action<string> ResponseReceived;

        public PunityClient(Guid id, ILogger logger)
        {
            Id = id;
            _logger = logger;
        }

        public void Start(ClientArguments startArguments)
        {
            if (!CanStartClient(startArguments))
                return;
            StartClientProcess();
        }

        public async Task<bool> StartAsync(ClientArguments startArguments, CancellationToken token = default)
        {
            if (!CanStartClient(startArguments))
                return false;
            StartClientProcess();
            while (!IsConnected && !HasExited && !token.IsCancellationRequested)
                await Task.Delay(50, token);
            return IsConnected && !HasExited && !token.IsCancellationRequested;
        }

        private bool CanStartClient(ClientArguments startArguments)
        {
            if (_isStarted)
            {
                _logger.LogWarning("Client has already been started!");
                return false;
            }

            _isStarted = true;
            _startArguments = startArguments;
            string message = null;
            if (_startArguments == null || !_startArguments.IsValid(out message))
                throw new ArgumentException(message ?? "StartArguments cannot be null!");
            return true;
        }

        private void StartClientProcess()
        {
            _myProcess = new Process();

            _myProcess.StartInfo.UseShellExecute = false;
            _myProcess.StartInfo.Verb = "runas";
            // var root = Application.dataPath.Replace("Assets", "");

            // _myProcess.StartInfo.FileName =
            //     Path.Combine(root, "Packages", "com.hamersoft.punity", "Plugins",
            //         $"PunityTCPClient{(Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor ? ".exe" : "")}");
            _myProcess.StartInfo.CreateNoWindow = true;
            _myProcess.StartInfo.FileName = Path.Combine(Application.streamingAssetsPath,
                $"PunityTCPClient{(Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor ? ".exe" : "")}");
            // _myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            _myProcess.StartInfo.Arguments =
                $"{_startArguments.Ip} {_startArguments.Port} \"{_startArguments.App}\" {Id}  \"{_startArguments.WorkingDirectory}\"";
            _myProcess.StartInfo.WorkingDirectory = _startArguments.WorkingDirectory;
            HasExited = false;
            _myProcess.Exited += ProcessExited;
            _myProcess.Start();
        }

        public void Connect(Stream stream)
        {
            if (!_isStarted)
            {
                _logger.LogError("Client has not yet started, cannot connect!");
            }

            if (HasExited)
            {
                _logger.LogWarning("Client has already exited! Cannot connect!");
                return;
            }

            if (IsConnected)
            {
                _logger.LogWarning("Client is already connected!");
                return;
            }

            if (stream == null)
            {
                _logger.LogWarning("Cannot connect to NULL stream!");
                return;
            }

            _stream = stream;
            StartReading();
        }

        private void StartReading()
        {
            const int readSize = 4096;
            byte[] buffer = new byte[readSize];
            _callback = null;
            _callback = ar =>
            {
                int bytesRead = _stream.EndRead(ar);
                if (bytesRead > 0)
                {
                    var output = _startArguments.Encoder.Read(buffer[..bytesRead]);
// populate foo
                    int i = buffer.Length - 1;
                    while(buffer[i] == 0)
                        --i;
// now foo[i] is the last non-zero byte
                    byte[] bar = new byte[i+1];
                    Array.Copy(buffer, bar, i+1);
                    OnResponseReceived(output);
                    OnBytesReceived(bar);
                }

                Array.Clear(buffer, 0, buffer.Length);
                if (!HasExited && IsConnected)
                    _stream.BeginRead(buffer, 0, readSize, _callback, this);
            };

            _stream.BeginRead(buffer, 0, readSize, _callback, this);
        }

        private void OnResponseReceived(string response)
        {
            ResponseReceived?.Invoke(response);
        }
        private void OnBytesReceived(byte[] response)
        {
            BytesReceived?.Invoke(response);
        }

        private void ProcessExited(object sender, EventArgs e)
        {
            _logger.LogWarning("Punity Client exited!");
            Stop();
        }

        public async Task Write(string text)
        {
            if (!IsConnected || HasExited || _stream == null)
            {
                _logger.LogWarning("Cannot write to client, make sure to start and connect!");
                return;
            }

            var bytes = _startArguments.Encoder.Write(text);
            await _stream.WriteAsync(bytes, 0, bytes.Length);
            await _stream.FlushAsync();
        }

        public async Task WriteLine(string text)
        {
            if (!IsConnected || HasExited || _stream == null)
            {
                _logger.LogWarning("Cannot write to client, make sure to start and connect!");
                return;
            }

            var bytes = _startArguments.Encoder.Write($"{Environment.NewLine}{text}");
            await _stream.WriteAsync(bytes, 0, bytes.Length);
            await _stream.FlushAsync();
        }

        public async Task Write(byte[] bytes)
        {
            if (!IsConnected || HasExited || _stream == null)
            {
                _logger.LogWarning("Cannot write to client, make sure to start and connect!");
                return;
            }

            await _stream.WriteAsync(bytes, 0, bytes.Length);
            await _stream.FlushAsync();
        }

        public void Stop()
        {
            if (_myProcess != null)
            {
                if (!_myProcess.HasExited)
                    _myProcess.Kill();
                _myProcess.Exited -= ProcessExited;
                _myProcess = null;
            }

            HasExited = true;
            _stream?.Close();
            _stream?.Dispose();
            _stream = null;
            _isStarted = false;

            Exited?.Invoke();
            _logger.Log("Client stopped!");
        }
    }
}