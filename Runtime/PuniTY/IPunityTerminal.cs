using System;
using System.Threading.Tasks;

namespace HamerSoft.PuniTY
{
    public interface IPunityTerminal : IDisposable
    {
        public event Action Stopped;
        public event Action<string> ResponseReceived;
        public bool IsRunning { get; }
        
        public void Start(StartArguments arguments, ITerminalUI ui = null);
        public void Stop();
        
        public Task Write(string text);
        public Task WriteLine(string text);
        public Task Write(byte[] bytes);
    }
}