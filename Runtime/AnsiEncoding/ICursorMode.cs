using System;
using HamerSoft.PuniTY.AnsiEncoding;

namespace AnsiEncoding
{
    public interface ICursorMode
    {
        public event Action<CursorMode> CursorModeChanged;
        internal void SetCursorMode(CursorMode mode);
    }
}