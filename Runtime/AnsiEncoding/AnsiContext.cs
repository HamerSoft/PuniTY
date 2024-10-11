using System.Collections.Generic;
using HamerSoft.PuniTY.AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.PointerModes;
using HamerSoft.PuniTY.AnsiEncoding.TerminalModes;
using HamerSoft.PuniTY.Logging;
using UnityEngine.WSA;

namespace AnsiEncoding
{
    public class AnsiContext : IAnsiContext
    {
       
        public IScreen Screen { get; private set; }
        public IPointer Pointer { get; private set; }
        public IReadOnlyList<ISequence> Sequences { get; private set; }
        ILogger IAnsiContext.Logger => _logger;
        ICursor IAnsiContext.Cursor => _cursor;
        private AnsiDecoder _ansiDecoder;
        private TerminalModeContext _terminalModeContext;
        private ILogger _logger;
        private ICursor _cursor;
        private IModeFactory _modeFactory;

        public AnsiContext(IPointer pointer, IScreenConfiguration screenConfiguration, ILogger logger,
            params ISequence[] sequences)
        {
            _cursor = new Cursor.Cursor(); 
            _logger = logger;
            Pointer = pointer;
            Sequences = sequences;
            _modeFactory = new ModeFactory();
            Screen = new Screen(_cursor, _logger, screenConfiguration);
            _terminalModeContext = new TerminalModeContext(this, new ModeFactory());
            _ansiDecoder = new AnsiDecoder(Screen, new EscapeCharacterDecoder(), sequences);
        }

       
    }
}