using System;

namespace HamerSoft.PuniTY.Tests.Editor
{
    public class MockUi : ITerminalUI
    {
        public event Action Closed;
        public event Action<string> Written;
        public event Action<string> WrittenLine;
        public event Action<byte[]> WrittenByte;
        public string WrittenText { get; private set; }
        public bool IsClosed { get; set; }
        
        public MockUi()
        {
            WrittenText = "";
        }

        public void Print(string message)
        {
            WrittenText += message;
        }

        public void Write(string message)
        {
            Written?.Invoke(message);
        }

        public void Write(Byte[] message)
        {
            WrittenByte?.Invoke(message);
        }

        public void WriteLine(string message)
        {
            WrittenLine?.Invoke($"{Environment.NewLine}{message}");
        }

        public void Close()
        {
            IsClosed = true;
            Closed?.Invoke();
        }
    }
}