using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;

namespace HamerSoft.PuniTY.Core.Client
{
    internal class PunityClient : IPunityClient
    {
        private Process _myProcess;
        private ClientArguments _args;

        public event Action Exited;
        public bool HasExited => _myProcess?.HasExited == true;

        void IPunityClient.Start(ClientArguments args)
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
        }

        public void Stop()
        {
            _myProcess?.Kill();
        }
    }
}