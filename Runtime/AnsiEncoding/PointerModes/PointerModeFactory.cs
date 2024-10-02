using System;
using AnsiEncoding;

namespace HamerSoft.PuniTY.AnsiEncoding.PointerModes
{
    internal class PointerModeFactory : IPointerModeFactory
    {
        IPointerMode IPointerModeFactory.Create(AnsiContext ansiContext, PointerMode pointerMode)
        {
            switch (pointerMode)
            {
                case PointerMode.NeverHide:
                    return new NeverHide();
                case PointerMode.HideIfNotTracking:
                    return new HideWhenTrackingDisabled(ansiContext.Screen);
                case PointerMode.AlwaysHideInWindow:
                    return new HideInWindow();
                case PointerMode.AlwaysHide:
                    return new AlwaysHide();
                default:
                    throw new ArgumentOutOfRangeException(nameof(pointerMode), pointerMode, null);
            }
        }
    }
}