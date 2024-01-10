using System.Net;

namespace HamerSoft.PuniTY
{
    public interface IPunityArguments
    {
        public IPAddress Ip { get; }
        public uint Port { get; }
        public string App { get; }
        public string WorkingDirectory { get; set; }
        public IEncoder Encoder { get; }
        public bool IsValid(out string message);
    }
}