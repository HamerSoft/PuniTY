using AnsiEncoding;

namespace HamerSoft.PuniTY.AnsiEncoding.TerminalModes
{
    public interface IModeFactory
    {
        internal IMode Create(AnsiMode mode, IAnsiContext context);
        internal IPointerMode Create(PointerMode pointerMode, IAnsiContext context);
    }
}