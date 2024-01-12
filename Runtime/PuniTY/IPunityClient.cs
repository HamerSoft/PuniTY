using System;
using System.IO;
using System.Threading.Tasks;

namespace HamerSoft.PuniTY.Core
{
    public interface IPunityClient
    {
        public Guid Id { get; }
        public bool IsConnected { get; }

        public event Action<string> ResponseReceived;
        public event Action Exited;
        public bool HasExited { get; }
        public Task Write(string text);
        public Task WriteLine(string text);
        public Task Write(byte[] bytes);

        public void Start(StartArguments args);
        public void Stop();
        public void Connect(Stream stream);
    }
}