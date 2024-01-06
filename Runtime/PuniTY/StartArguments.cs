using System.Net;

namespace HamerSoft.PuniTY
{
    public class StartArguments : IPunityArguments
    {
        private string _ip;
        public IPAddress Ip { get; }
        public uint Port { get; }

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
                message = "Invalid Ip address!";
            return message == null;
        }
    }
}