using System.Net;

namespace HamerSoft.PuniTY
{
    public interface IPunityArguments
    {
        public IPAddress Ip { get; }
        public uint Port { get; }
        public bool IsValid(out string message);
    }
}