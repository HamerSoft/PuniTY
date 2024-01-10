using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;

namespace HamerSoft.PuniTY.Core.Client
{
    internal class PunityClient : IPunityClient
    {
        private Process _myProcess;
        private StartArguments _args;
        private readonly ILogger _logger;

        public event Action Exited;
        public bool HasExited => _myProcess?.HasExited == true;

        public PunityClient(ILogger logger)
        {
            _logger = logger;
        }

        void IPunityClient.Start(StartArguments args)
        {
            _args = args;
            if (!_args.IsValid(out var message))
                throw new ArgumentException(message);

            _myProcess = new Process();

            _myProcess.StartInfo.UseShellExecute = false;
            _myProcess.StartInfo.Verb = "runas";
            var root = Application.dataPath.Replace("Assets", "");

            // _myProcess.StartInfo.FileName =
            //     Path.Combine(root, "Packages", "com.hamersoft.punity", "Plugins",
            //         $"PunityTCPClient{(Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor ? ".exe" : "")}");
            _myProcess.StartInfo.CreateNoWindow = true;
            _myProcess.StartInfo.FileName = Path.Combine(Application.streamingAssetsPath,
                $"PunityTCPClient{(Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor ? ".exe" : "")}");
            // _myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            _myProcess.StartInfo.Arguments = $"{_args.Ip} {_args.Port}";
            _myProcess.StartInfo.WorkingDirectory = Environment.CurrentDirectory;
            _myProcess.Start();
            _myProcess.Exited += ProcessExited;
        }

        private void ProcessExited(object sender, EventArgs e)
        {
            _logger.LogError("Punity Client exited!");
            _myProcess.Exited -= ProcessExited;
            Exited?.Invoke();
        }

        public void Stop()
        {
            _myProcess?.Kill();
            _myProcess.Exited -= ProcessExited;
            Exited?.Invoke();
        }
    }
}