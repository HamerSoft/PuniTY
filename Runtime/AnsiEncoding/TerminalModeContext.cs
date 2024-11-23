using System;
using System.Collections.Generic;
using AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.TerminalModes.Modes;
using HamerSoft.PuniTY.Logging;

namespace HamerSoft.PuniTY.AnsiEncoding.TerminalModes
{
    public sealed class TerminalModeContext : ITerminalModeContext
    {
        private readonly IAnsiContext _context;
        private Dictionary<AnsiMode, IMode> _activeModes;
        private readonly IModeFactory _modeFactory;
        private readonly ILogger _logger;
        private PointerMode _pointerMode;
        private HashSet<PointerTrackingMode> _activePointerModes;

        public event Action<AnsiMode, bool> ModeChanged;
        public event Action<IPointerMode> PointerModeChanged;

        internal TerminalModeContext(IAnsiContext context, IModeFactory modeFactory)
        {
            _context = context;
            _logger = _context.Logger;
            _modeFactory = modeFactory;
            _activeModes = new Dictionary<AnsiMode, IMode>();
            _activePointerModes = new HashSet<PointerTrackingMode>();
        }

        void IModeable.SetMode(AnsiMode mode)
        {
            if (_activeModes.ContainsKey(mode))
            {
                _logger.LogWarning($"Mode: {mode} already active.");
                return;
            }

            var iMode = _modeFactory.Create(mode, _context);
            if (iMode != null)
            {
                _activeModes.Add(mode, iMode);
                iMode.Enable();
                ModeChanged?.Invoke(mode, true);
            }
            else
            {
                _logger.LogWarning($"No implementation for terminal mode {mode}. Skipping command...");
            }
        }

        void IModeable.ResetMode(AnsiMode mode)
        {
            if (_activeModes.Remove(mode, out var iMode))
            {
                iMode.Disable();
                ModeChanged?.Invoke(mode, false);
                iMode.Dispose();
            }
            else
                _logger.LogWarning($"Mode: {mode} is not active.");
        }

        public bool HasMode(params AnsiMode[] mode)
        {
            for (int i = 0; i < mode.Length; i++)
                if (!_activeModes.ContainsKey(mode[i]))
                    return false;
            return true;
        }

        void IPointerable.SetPointerMode(PointerMode mode)
        {
            if (mode == _pointerMode)
                return;
            _pointerMode = mode;
            PointerModeChanged?.Invoke(_modeFactory.Create(mode));
        }
    }
}