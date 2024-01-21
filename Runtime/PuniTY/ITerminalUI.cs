using System;

namespace HamerSoft.PuniTY
{
    public interface ITerminalUI
    {
        public event Action Closed;
        public event Action<string> Written;
        public event Action<byte[]> WrittenByte;
        public event Action<string> WrittenLine;

        public void Print(string message);
    }
}