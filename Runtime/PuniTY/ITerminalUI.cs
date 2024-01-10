using System;

namespace HamerSoft.PuniTY
{
    public interface ITerminalUI
    {
        public event Action<string> Written;
        public event Action<string> WrittenLine;
        public event Action<byte[]> WrittenByte;

        public string Print(string message);
    }
}