using System;

namespace HamerSoft.PuniTY
{
    public interface IPunityTerminal : IDisposable
    {
        public event Action Stopped;
        public bool IsRunning { get; }
        public void Start(StartArguments arguments, ITerminalUI ui = null);
        public void Stop();
    }
}