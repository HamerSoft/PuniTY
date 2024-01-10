using System;
using System.IO;
using System.Net;

namespace HamerSoft.PuniTY
{
    public class StartArguments : IPunityArguments
    {
        public IPAddress Ip { get; }
        public uint Port { get; }
        public string App { get; }
        public string WorkingDirectory { get; set; }
        public IEncoder Encoder { get; }

        private string _ip;

        public StartArguments(string ip, uint port, string app, IEncoder encoder,
            string workingDirectory = null)
        {
            _ip = ip;
            Encoder = encoder;
            if (IPAddress.TryParse(ip, out var ipAddress))
                Ip = ipAddress;
            App = app;
            WorkingDirectory = !string.IsNullOrWhiteSpace(workingDirectory)
                ? workingDirectory
                : Environment.CurrentDirectory;
            Port = port;
        }

        public virtual bool IsValid(out string message)
        {
            message = null;
            if (Ip == null)
                message = $"Invalid Ip address: {_ip}!";
            else if (string.IsNullOrWhiteSpace(App) || !File.Exists(App))
                message = "Please specify app to start like cmd or bash!";
            else if (Encoder == null)
                message =
                    "No encoding specified! Take the default HamerSoft.PuniTY.AnsiEncoder for ANSI VT100 terminals.";
            return message == null;
        }
    }
}