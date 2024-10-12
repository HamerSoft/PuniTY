using HamerSoft.PuniTY.AnsiEncoding.TerminalModes;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    public class ResetModeSequence : ModeSequence
    {
        public override char Command => 'l';
        
        protected override void SetMode(IModeable modeable, AnsiMode mode)
        {
            modeable.ResetMode(mode);
        }
    }
}