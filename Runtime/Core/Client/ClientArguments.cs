using System;

namespace HamerSoft.PuniTY.Core.Client
{
    internal class ClientArguments : StartArguments
    {
        public string Ip { get; }
        public int Port { get; }

        public string App { get; private set; }
        public string WorkingDirectory { get; private set; }

        public ClientArguments(string ip, uint port, string app, string workingDirectory = null) : base(ip, port)
        {
            App = app;
            WorkingDirectory = workingDirectory ?? Environment.CurrentDirectory;
        }

        public override bool IsValid(out string message)
        {
            var isValid = base.IsValid(out message);
            if (isValid && string.IsNullOrWhiteSpace(App))
            {
                message = "Please specify app to start like cmd or bash!";
            }

            return message == null;
        }
    }
}