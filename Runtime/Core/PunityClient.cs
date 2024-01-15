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

        public bool IsConnected => _stream != null;
        public event Action<string> ResponseReceived;
        public Guid Id { get; private set; }
        public event Action Exited;
        public bool HasExited => _myProcess?.HasExited == true;

        public PunityClient(Guid id, ILogger logger)
        {
            Id = id;
            _logger = logger;
        }

        public void Start(ClientArguments startArguments)
        {
            _startArguments = startArguments;
            if (!_startArguments.IsValid(out var message))
                throw new ArgumentException(message);

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
            _myProcess.Exited += ProcessExited;
            _myProcess.Start();
        }

        public void Connect(Stream stream)
        {
            _stream = stream;
            StartReading();
        }

        private void StartReading()
        {
            const int readSize = 4096;
            byte[] buffer = new byte[readSize];
            AsyncCallback callback = null;
            callback = ar =>
            {
                int bytesRead = _stream.EndRead(ar);
                if (bytesRead < 0)
                    return;

                var output = _startArguments.Encoder.Read(buffer[..bytesRead]);

                OnResponseReceived(output);

                Array.Clear(buffer, 0, buffer.Length);
                _stream.BeginRead(buffer, 0, readSize, callback, this);
            };

            _stream.BeginRead(buffer, 0, readSize, callback, this);
        }

        private void OnResponseReceived(string response)
        {
            ResponseReceived?.Invoke(response);
        }

        private void ProcessExited(object sender, EventArgs e)
        {
            _logger.LogError("Punity Client exited!");
            _myProcess.Exited -= ProcessExited;
            Exited?.Invoke();
        }

        public async Task Write(string text)
        {
            var bytes = _startArguments.Encoder.Write(text);
            await _stream.WriteAsync(bytes, 0, bytes.Length);
        }

        public async Task WriteLine(string text)
        {
            var bytes = _startArguments.Encoder.Write($"{Environment.NewLine}{text}");
            await _stream.WriteAsync(bytes, 0, bytes.Length);
        }

        public async Task Write(byte[] bytes)
        {
            await _stream.WriteAsync(bytes, 0, bytes.Length);
        }

        public void Stop()
        {
            if (_myProcess != null)
            {
                _myProcess.Kill();
                _myProcess.Exited -= ProcessExited;
            }

            _stream?.Close();
            _stream?.Dispose();
            _stream = null;
            Exited?.Invoke();
        }
    }
}