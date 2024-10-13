using AnsiEncoding;

namespace HamerSoft.PuniTY.AnsiEncoding.TerminalModes.Modes
{
    internal abstract class TerminalMode : IMode
    {
        protected IAnsiContext AnsiContext;

        protected TerminalMode(IAnsiContext ansiContext)
        {
            AnsiContext = ansiContext;
        }

        public abstract void Enable();
        public abstract void Disable();

        public virtual void Apply()
        {
        }

        public void Dispose()
        {
        }
    }
}