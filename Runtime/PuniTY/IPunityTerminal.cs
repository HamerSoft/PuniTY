using System;
using System.Threading;
using System.Threading.Tasks;
using HamerSoft.PuniTY.Configuration;

namespace HamerSoft.PuniTY
{
    public interface IPunityTerminal
    {
        public bool IsRunning { get; }

        public event Action Stopped;
        public event Action<string> ResponseReceived;

        public void Start(ClientArguments arguments, ITerminalUI ui = null);
        public Task StartAsync(ClientArguments arguments, ITerminalUI ui = null, CancellationToken token = default);
        public void Stop();
        public Task Write(string text);
        public Task WriteLine(string text);
        public Task Write(byte[] bytes);
    }
}