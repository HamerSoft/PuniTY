using System;
using System.Collections.Generic;
using System.Text;
using AnsiEncoding.Input;
using HamerSoft.PuniTY.AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.TerminalModes;
using HamerSoft.PuniTY.Logging;

namespace AnsiEncoding
{
    public class AnsiContext : IAnsiContext
    {
        public IScreen Screen { get; private set; }
        public IPointer Pointer { get; private set; }
        public IKeyboard Keyboard { get; private set; }
        public ITerminalModeContext TerminalModeContext { get; private set; }
        public IEscapeCharacterDecoder Decoder { get; }

        ILogger IAnsiContext.Logger => _logger;
        IInputTransmitter IAnsiContext.InputTransmitter => _inputTransmitter;

        private AnsiDecoder _ansiDecoder;
        private ILogger _logger;
        private ICursor _cursor;
        private bool _isDisposed;
        private readonly InputTransmitter _inputTransmitter;

        public AnsiContext(IInput input, IScreenConfiguration screenConfiguration, ILogger logger,
            params ISequence[] sequences) : this(input, screenConfiguration, logger, new ModeFactory(), sequences)
        {
        }

        internal AnsiContext(IInput input, IScreenConfiguration screenConfiguration, ILogger logger,
            IModeFactory modeFactory,
            params ISequence[] sequences)
        {
            _cursor = new Cursor.Cursor();
            _logger = logger;
            Decoder = new EscapeCharacterDecoder();
            _ansiDecoder = new AnsiDecoder(Decoder, ExecuteSequence, ProcessOutput, sequences);
            TerminalModeContext = new TerminalModeContext(this, modeFactory);
            Pointer = input.Pointer;
            Keyboard = input.KeyBoard;
            Screen = new Screen(_cursor, _logger, screenConfiguration);
            TerminalModeContext.PointerModeChanged += Pointer.SetMode;
            _inputTransmitter = new InputTransmitter(input, new CellReportStrategy(Pointer, Screen));
        }

        private void ProcessOutput(byte[] output)
        {
            foreach (byte b in output)
                Screen.AddCharacter((char)b);
        }

        private void ExecuteSequence(ISequence sequence, string parameters)
        {
            if (_isDisposed)
                return;
            sequence.Execute(this, parameters);
        }

        public void Log(string message)
        {
            if (_isDisposed)
                return;
            _logger.Log(message);
        }

        public void LogWarning(string message)
        {
            if (_isDisposed)
                return;
            _logger.LogWarning(message);
        }

        public void LogError(string message)
        {
            if (_isDisposed)
                return;
            _logger.LogError(message);
        }

        public void LogError(string message, Exception exception)
        {
            if (_isDisposed)
                return;
            _logger.LogError($"{message} | {exception} -> {exception.Message}");
        }

        public void Dispose()
        {
            if (_isDisposed)
                return;
            _isDisposed = true;
            _logger.Log("Disposing AnsiContext.");
            TerminalModeContext.PointerModeChanged -= Pointer.SetMode;
            _inputTransmitter.Dispose();
            _ansiDecoder.Dispose();
            _ansiDecoder = null;
            Decoder.Dispose();
            Screen = null;
            Pointer = null;
            _cursor = null;
            TerminalModeContext = null;
        }
    }
}