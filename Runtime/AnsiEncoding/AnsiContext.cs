﻿using System;
using System.Collections.Generic;
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
        InputTransmitter IAnsiContext.InputTransmitter => _inputTransmitter;

        private AnsiDecoder _ansiDecoder;
        private IReadOnlyList<ISequence> _sequences;
        private ILogger _logger;
        private ICursor _cursor;
        private bool _isDisposed;
        private readonly InputTransmitter _inputTransmitter;

        public AnsiContext(IInput input, IScreenConfiguration screenConfiguration, ILogger logger,
            params ISequence[] sequences)
        {
            _cursor = new Cursor.Cursor();
            _logger = logger;
            _sequences = sequences;
            Decoder = new EscapeCharacterDecoder();
            _ansiDecoder = new AnsiDecoder(Decoder, ExecuteSequence, sequences);
            TerminalModeContext = new TerminalModeContext(this, new ModeFactory());
            Pointer = input.Pointer;
            Keyboard = input.KeyBoard;
            TerminalModeContext.PointerModeChanged += Pointer.SetMode;
            Screen = new Screen(_cursor, _logger, screenConfiguration);
            _inputTransmitter = new InputTransmitter(input, Screen);
        }

        internal AnsiContext(IPointer pointer, IScreenConfiguration screenConfiguration, ILogger logger,
            IModeFactory modeFactory,
            params ISequence[] sequences)
        {
            _cursor = new Cursor.Cursor();
            _logger = logger;
            _sequences = sequences;
            Decoder = new EscapeCharacterDecoder();
            _ansiDecoder = new AnsiDecoder(Decoder, ExecuteSequence, sequences);
            TerminalModeContext = new TerminalModeContext(this, modeFactory);
            Pointer = pointer;
            TerminalModeContext.PointerModeChanged += pointer.SetMode;
            Screen = new Screen(_cursor, _logger, screenConfiguration);
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
            _sequences = Array.Empty<ISequence>();
            Screen = null;
            Pointer = null;
            _cursor = null;
            TerminalModeContext = null;
        }
    }
}