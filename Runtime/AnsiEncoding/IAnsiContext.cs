using System.Collections.Generic;
using HamerSoft.PuniTY.AnsiEncoding;
using HamerSoft.PuniTY.Logging;

namespace AnsiEncoding
{
    public interface IAnsiContext
    {
        public IScreen Screen { get; }
        public IPointer Pointer { get; }
        public IReadOnlyList<ISequence> Sequences { get; }
        internal ILogger Logger { get; }
        internal ICursor Cursor { get; }
    }
}