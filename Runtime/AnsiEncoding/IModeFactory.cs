namespace HamerSoft.PuniTY.AnsiEncoding.TerminalModes
{
    public interface IModeFactory
    {
        internal IMode Create(AnsiMode mode, IScreen screen);
    }
}