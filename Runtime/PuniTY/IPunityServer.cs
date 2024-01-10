using System;
using System.Threading.Tasks;

namespace HamerSoft.PuniTY
{
    public interface IPunityServer : IDisposable
    {
        public event Action<string> ResponseReceived;
        public event Action ConnectionLost;
        public bool IsConnected { get; }

        public void Start(StartArguments args);
        public void Stop();
        public Task Write(string text);
        public Task WriteLine(string text);
        public Task Write(byte[] bytes);
    }
}