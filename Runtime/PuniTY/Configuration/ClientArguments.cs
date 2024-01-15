using System;
using System.IO;
using HamerSoft.PuniTY.Encoding;

namespace HamerSoft.PuniTY.Configuration
{
    public class ClientArguments : StartArguments
    {
        public string App { get; }
        public string WorkingDirectory { get; }
        public IEncoder Encoder { get; }

        public ClientArguments(StartArguments startArguments, string app, IEncoder encoder,
            string workingDirectory = null) : base(startArguments.Ip, startArguments.Port)
        {
            Encoder = encoder;
            App = app;
            WorkingDirectory = !string.IsNullOrWhiteSpace(workingDirectory)
                ? workingDirectory
                : Environment.CurrentDirectory;
        }

        public ClientArguments(string ip, uint port, string app, IEncoder encoder,
            string workingDirectory = null) : base(ip, port)
        {
            Encoder = encoder;
            App = app;
            WorkingDirectory = !string.IsNullOrWhiteSpace(workingDirectory)
                ? workingDirectory
                : Environment.CurrentDirectory;
        }

        public override bool IsValid(out string message)
        {
            var isValid = base.IsValid(out message);
            if (string.IsNullOrWhiteSpace(App) || !File.Exists(App))
                message = "Please specify app to start like cmd, bash or powershell!";
            else if (Encoder == null)
                message =
                    "No encoding specified! Take the default HamerSoft.PuniTY.AnsiEncoder for ANSI VT100 terminals.";
            else if (!Directory.Exists(WorkingDirectory))
                message = $"Directory {WorkingDirectory} does not exist!";

            return isValid && message == null;
        }
    }
}