﻿using HamerSoft.PuniTY.AnsiEncoding;

namespace AnsiEncoding
{
    public interface ITerminalModeContext : IModeable, IPointerable
    {
        internal bool IsMouseTrackingEnabled();
    }
}