using System.Collections.Generic;
using HamerSoft.PuniTY.AnsiEncoding.TerminalModes;

namespace HamerSoft.PuniTY.AnsiEncoding.PointerModes
{
    internal class HideWhenTrackingDisabled : IPointerMode
    {
        private readonly IScreen _screen;
        public PointerMode Mode => PointerMode.HideIfNotTracking;
        private HashSet<AnsiMode> _acceptedModes;

        public HideWhenTrackingDisabled(IScreen screen)
        {
            _screen = screen;
            _screen.ModeChanged += ScreenOnModeChanged;
            _acceptedModes = new HashSet<AnsiMode>()
            {
                AnsiMode.SendMouseXY,
                AnsiMode.UseAllMotionMouseTracking,
                AnsiMode.UseCellMotionMouseTracking,
                AnsiMode.UseHiliteMouseTracking,
                AnsiMode.UrxvtMouse,
                AnsiMode.EnableSGRMouse,
                AnsiMode.SendMouseXYOnButtonPressAndRelease
            };
        }

        private void ScreenOnModeChanged(AnsiMode mode, bool enabled)
        {
            if (enabled)
            {
                if (_acceptedModes.Contains(mode))
                    
            }
        }

        void IPointerMode.Apply(IPointer pointer, Rect _)
        {
            if (_screen.HasMode(AnsiMode.SendMouseXY)
                || _screen.HasMode(AnsiMode.UseAllMotionMouseTracking)
                || _screen.HasMode(AnsiMode.UseCellMotionMouseTracking)
                || _screen.HasMode(AnsiMode.UseHiliteMouseTracking)
                || _screen.HasMode(AnsiMode.UrxvtMouse)
                || _screen.HasMode(AnsiMode.EnableSGRMouse)
                || _screen.HasMode(AnsiMode.SendMouseXYOnButtonPressAndRelease))
                pointer.Show();
            else
                pointer.Hide();
        }
    }
}