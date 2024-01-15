using System;
using System.IO;
using System.Threading.Tasks;
using HamerSoft.PuniTY.Configuration;

namespace HamerSoft.PuniTY
{
    public interface IPunityClient
    {
        public Guid Id { get; }
        public bool IsConnected { get; }
        public bool HasExited { get; }

        public event Action<string> ResponseReceived;
        public event Action Exited;

        public Task Write(string text);
        public Task WriteLine(string text);
        public Task Write(byte[] bytes);

        public void Start(ClientArguments startArguments);
        public void Stop();
        public void Connect(Stream stream);
    }
}