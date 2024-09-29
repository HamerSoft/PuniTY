using HamerSoft.PuniTY.AnsiEncoding.TerminalModes;

namespace HamerSoft.PuniTY.AnsiEncoding.PointerModes
{
    internal class HideWhenTrackingDisabled : IPointerMode
    {
        private readonly IScreen _screen;
        public PointerMode Mode => PointerMode.HideIfNotTracking;

        public HideWhenTrackingDisabled(IScreen screen)
        {
            _screen = screen;
        }

        void IPointerMode.Apply(IPointer pointer, Rect _)
        {
            if (_screen.HasMode(AnsiMode.SendMouseXY))
                pointer.Show();
            else
                pointer.Hide();
        }
    }
}