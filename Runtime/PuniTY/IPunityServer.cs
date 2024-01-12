using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using HamerSoft.PuniTY.Core;

namespace HamerSoft.PuniTY
{
    public interface IPunityServer : IDisposable
    {
        public event Action<Guid> ConnectionLost;
        public event Action<Guid, Stream> ClientConnected;
        public bool IsConnected { get; }
        public int ConnectedClients { get; }

        public void Start(StartArguments startArguments);
        public void Stop(IPunityClient client);
        public void Stop();
    }
}