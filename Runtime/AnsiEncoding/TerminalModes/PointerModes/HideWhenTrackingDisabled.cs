using System;
using HamerSoft.PuniTY.AnsiEncoding.TerminalModes;

namespace HamerSoft.PuniTY.AnsiEncoding.PointerModes
{
    internal class HideWhenTrackingDisabled : IPointerMode
    {
        public PointerMode Mode => PointerMode.HideIfNotTracking;

        void IPointerMode.Apply(IPointer pointer, Rect _)
        {
            if (pointer.IsTrackingEnabled)
                pointer.Show();
            else
                pointer.Hide();
        }
    }
}