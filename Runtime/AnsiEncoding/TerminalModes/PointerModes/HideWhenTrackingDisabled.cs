using System;
using HamerSoft.PuniTY.AnsiEncoding.TerminalModes;

namespace HamerSoft.PuniTY.AnsiEncoding.PointerModes
{
    internal class HideWhenTrackingDisabled : IPointerMode
    {
        private readonly IPointer _pointer;
        private readonly Func<bool> _checkMouseTrackingEnabled;
        public PointerMode Mode => PointerMode.HideIfNotTracking;

        public HideWhenTrackingDisabled(IPointer pointer, Func<bool> checkMouseTrackingEnabled)
        {
            _pointer = pointer;
            _checkMouseTrackingEnabled = checkMouseTrackingEnabled;
            // _acceptedModes = new HashSet<AnsiMode>()
            // {
            //     AnsiMode.SendMouseXY,
            //     AnsiMode.UseAllMotionMouseTracking,
            //     AnsiMode.UseCellMotionMouseTracking,
            //     AnsiMode.UseHiliteMouseTracking,
            //     AnsiMode.UrxvtMouse,
            //     AnsiMode.EnableSGRMouse,
            //     AnsiMode.SendMouseXYOnButtonPressAndRelease
            // };
        }

        private void ScreenOnModeChanged(AnsiMode mode, bool enabled)
        {
            if (_checkMouseTrackingEnabled())
                _pointer.Show();
            else
                _pointer.Hide();
        }

        void IPointerMode.Apply(IPointer pointer, Rect _)
        {
            if (_checkMouseTrackingEnabled())
                pointer.Show();
            else
                pointer.Hide();
        }
    }
}