using HamerSoft.PuniTY.AnsiEncoding.TerminalModes;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    public class SetModeSequence : ModeSequence
    {
        public override char Command => 'h';

        protected override void SetMode(IModeable modeable, AnsiMode mode)
        {
            modeable.SetMode(mode);
        }
    }
}