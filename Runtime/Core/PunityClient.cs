using System;
using System.Diagnostics;
using System.IO;
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

        public Guid Id { get; private set; }
        public bool IsConnected => _stream != null;
        public bool HasExited { get; protected set; }
        public event Action Exited;
        public event Action<string> ResponseReceived;

        public PunityClient(Guid id, ILogger logger)
        {
            Id = id;
            _logger = logger;
        }

        public virtual void Start(ClientArguments startArguments)
        {
            _startArguments = startArguments;
            string message = null;
            if (_startArguments == null || !_startArguments.IsValid(out message))
                throw new ArgumentException(message ?? "StartArguments cannot be null!");

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
            if (HasExited)
            {
                _logger.LogWarning("Client has already exited! Cannot connect!");
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
                    OnResponseReceived(output);
                }

                if (!HasExited && IsConnected)
                    _stream.BeginRead(buffer, 0, readSize, _callback, this);
            };

            _stream.BeginRead(buffer, 0, readSize, _callback, this);
        }

        private void OnResponseReceived(string response)
        {
            ResponseReceived?.Invoke(response);
        }

        private void ProcessExited(object sender, EventArgs e)
        {
            _logger.LogError("Punity Client exited!");
            _myProcess.Exited -= ProcessExited;
            HasExited = true;
            _stream?.Close();
            _stream?.Dispose();
            _stream = null;
            Exited?.Invoke();
        }

        public async Task Write(string text)
        {
            var bytes = _startArguments.Encoder.Write(text);
            await _stream.WriteAsync(bytes, 0, bytes.Length);
            await _stream.FlushAsync();
        }

        public async Task WriteLine(string text)
        {
            var bytes = _startArguments.Encoder.Write($"{Environment.NewLine}{text}");
            await _stream.WriteAsync(bytes, 0, bytes.Length);
            await _stream.FlushAsync();
        }

        public async Task Write(byte[] bytes)
        {
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

            Exited?.Invoke();
        }
    }
}