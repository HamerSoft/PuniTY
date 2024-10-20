using System;

namespace AnsiEncoding.Input
{
    public interface IInputTransmitter : IDisposable
    {
        internal event Action<byte[]> Output;

        public void Transmit(string data);
        public void Transmit(byte[] data);

        internal void SetMouseReportingMode(PointerReportStrategy pointerReportStrategy);
    }
}