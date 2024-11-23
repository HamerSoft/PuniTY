using System.Numerics;
using AnsiEncoding;
using AnsiEncoding.Input;
using HamerSoft.PuniTY.AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.PointerModes;
using HamerSoft.PuniTY.AnsiEncoding.TerminalModes;
using HamerSoft.PuniTY.Logging;
using Tests.Editor.AnsiDecoding.Stubs;

namespace HamerSoft.PuniTY.Tests.Editor.AnsiDecoding.Stubs
{
    public class StubAnsiContext : AnsiContext
    {
        public StubAnsiContext(IInput input, IScreenConfiguration screenConfiguration, ILogger logger,
            params ISequence[] sequences) : base(input, screenConfiguration, logger, sequences)
        {
            Logger = logger;
        }

        public StubAnsiContext(int rows, int columns, ILogger logger, params ISequence[] sequences) : base(
            new StubInput(new StubPointer(new NeverHide(), new Vector2(0, 0), new Rect(0, 0, 100, 100)),
                new StubKeyboard()),
            new Screen.DefaultScreenConfiguration(rows, columns, 8, new FontDimensions(10, 10)), logger, sequences)
        {
            Logger = logger;
        }

        public StubAnsiContext(int rows, int columns, ILogger logger, IModeFactory modeFactory,
            params ISequence[] sequences) : base(
            new StubInput(new StubPointer(new NeverHide(), new Vector2(0, 0), new Rect(0, 0, 100, 100)),
                new StubKeyboard()),
            new Screen.DefaultScreenConfiguration(rows, columns, 8, new FontDimensions(10, 10)), logger, modeFactory,
            sequences)
        {
            Logger = logger;
        }

        internal ILogger Logger { get; }
        internal ICursor Cursor => Screen.Cursor;
        internal IInputTransmitter InputTransmitter => ((IAnsiContext)this).InputTransmitter;
    }
}