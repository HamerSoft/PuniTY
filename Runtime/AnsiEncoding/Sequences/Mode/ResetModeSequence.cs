using HamerSoft.PuniTY.AnsiEncoding.TerminalModes;
using HamerSoft.PuniTY.Logging;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    public class ResetModeSequence : ModeSequence
    {
        public override char Command => 'l';

        public ResetModeSequence(ILogger logger) : base(logger)
        {
        }

        protected override void SetMode(IScreen screen, AnsiMode mode)
        {
            screen.ResetMode(mode);
        }
    }
}