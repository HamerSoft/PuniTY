using HamerSoft.PuniTY.AnsiEncoding.TerminalModes;
using HamerSoft.PuniTY.Logging;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    public class SetModeSequence : ModeSequence
    {
        public override char Command => 'h';

        public SetModeSequence(ILogger logger) : base(logger)
        {
        }
        
        protected override void SetMode(IScreen screen, AnsiMode mode)
        {
            screen.SetMode(mode);
        }
    }
}