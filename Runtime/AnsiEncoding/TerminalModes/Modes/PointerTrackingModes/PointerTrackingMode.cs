using AnsiEncoding;

namespace HamerSoft.PuniTY.AnsiEncoding.TerminalModes.Modes
{
    internal abstract class PointerTrackingMode : TerminalMode
    {
        protected PointerTrackingMode(IAnsiContext ansiContext) : base(ansiContext)
        {
        }

        public override void Enable()
        {
            AnsiContext.Pointer.EnableTracking();
        }

        public override void Disable()
        {
            AnsiContext.Pointer.DisableTracking();
        }
    }
}