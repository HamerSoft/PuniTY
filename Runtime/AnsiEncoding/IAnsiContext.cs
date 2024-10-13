using System;
using System.Collections.Generic;
using HamerSoft.PuniTY.AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.TerminalModes;
using HamerSoft.PuniTY.Logging;

namespace AnsiEncoding
{
    public interface IAnsiContext : ILogger, IDisposable
    {
        public IScreen Screen { get; }
        public IPointer Pointer { get; }
        public IKeyboard Keyboard { get; }
        public ITerminalModeContext TerminalModeContext { get; }
        public IEscapeCharacterDecoder Decoder { get; }
        internal ILogger Logger { get; }
        internal IReadOnlyList<ISequence> Sequences { get; }
    }
}