using System;
using HamerSoft.PuniTY.Logging;

namespace HamerSoft.PuniTY.UI
{
    public class TerminalUI : ITerminalUI
    {
        private readonly ILogger _logger;
        
        public event Action<string> Written;
        public event Action<string> WrittenLine;
        public event Action<byte[]> WrittenByte;

        public TerminalUI(ILogger logger)
        {
            _logger = logger;
        }
        
        public string Print(string message)
        {
            throw new NotImplementedException();
        }
    }
}