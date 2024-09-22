namespace HamerSoft.PuniTY.AnsiEncoding.TerminalModes
{
    public interface IModeFactory
    {
        public IMode Create(AnsiMode mode, IScreen screen);
    }
}