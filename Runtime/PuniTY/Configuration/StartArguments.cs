using System.Net;

namespace HamerSoft.PuniTY.Configuration
{
    public class StartArguments : IPunityArguments
    {
        public IPAddress Ip { get; }
        public uint Port { get; }
       

        private string _ip;

        public StartArguments(string ip, uint port)
        {
            _ip = ip;
            if (IPAddress.TryParse(ip, out var ipAddress))
                Ip = ipAddress;
            Port = port;
        }

        public virtual bool IsValid(out string message)
        {
            message = null;
            if (Ip == null)
                message = $"Invalid Ip address: {_ip}!";
            return message == null;
        }
    }
}