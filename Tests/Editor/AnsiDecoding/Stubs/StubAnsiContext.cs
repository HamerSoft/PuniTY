using System.Numerics;
using AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.PointerModes;
using HamerSoft.PuniTY.AnsiEncoding.TerminalModes;
using HamerSoft.PuniTY.Logging;

namespace HamerSoft.PuniTY.Tests.Editor.AnsiDecoding.Stubs
{
    public class StubAnsiContext : AnsiContext
    {
        public StubAnsiContext(IPointer pointer, IScreenConfiguration screenConfiguration, ILogger logger,
            params ISequence[] sequences) : base(pointer, screenConfiguration, logger, sequences)
        {
            Logger = logger;
        }

        public StubAnsiContext(int rows, int columns, ILogger logger, params ISequence[] sequences) : base(
            new StubPointer(new NeverHide(), new Vector2(0, 0), new Rect(0, 0, 100, 100)),
            new Screen.DefaultScreenConfiguration(rows, columns, 8), logger, sequences)
        {
            Logger = logger;
        }

        public StubAnsiContext(int rows, int columns, ILogger logger, IModeFactory modeFactory,
            params ISequence[] sequences) : base(
            new StubPointer(new NeverHide(), new Vector2(0, 0), new Rect(0, 0, 100, 100)),
            new Screen.DefaultScreenConfiguration(rows, columns, 8), logger, modeFactory, sequences)
        {
            Logger = logger;
        }

        internal ILogger Logger { get; }
        internal ICursor Cursor => Screen.Cursor;
    }
}