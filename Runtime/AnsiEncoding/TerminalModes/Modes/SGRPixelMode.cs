using AnsiEncoding;
using AnsiEncoding.Input;

namespace HamerSoft.PuniTY.AnsiEncoding.TerminalModes.Modes
{
    internal class SGRPixelMode : TerminalMode
    {
        public SGRPixelMode(IAnsiContext ansiContext) : base(ansiContext)
        {
        }

        public override void Enable()
        {
            AnsiContext.InputTransmitter.SetMouseReportingMode(new PixelReportStrategy(AnsiContext.Pointer));
        }

        public override void Disable()
        {
            AnsiContext.InputTransmitter.SetMouseReportingMode(new CellReportStrategy(AnsiContext.Pointer));
        }
    }
}