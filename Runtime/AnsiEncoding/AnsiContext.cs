using System.Collections.Generic;
using HamerSoft.PuniTY.AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.PointerModes;
using HamerSoft.PuniTY.AnsiEncoding.TerminalModes;
using HamerSoft.PuniTY.Logging;
using UnityEngine.WSA;

namespace AnsiEncoding
{
    public class AnsiContext
    {
        public IScreen Screen;
        public IPointer Pointer;
        public IReadOnlyList<ISequence> Sequences;
        internal IModeFactory ModeFactory;
        internal ILogger Logger;
        internal ICursor Cursor;
        private AnsiDecoder _ansiDecoder;

        public AnsiContext(IPointer pointer, IScreenConfiguration screenConfiguration, ILogger logger,
            params ISequence[] sequences)
        {
            Cursor = new Cursor.Cursor();
            Logger = logger;
            Pointer = pointer;
            Sequences = sequences;
            Screen = new Screen(Cursor, Logger, screenConfiguration, new ModeFactory());
            _ansiDecoder = new AnsiDecoder(Screen, new EscapeCharacterDecoder(), sequences);
        }
    }
}