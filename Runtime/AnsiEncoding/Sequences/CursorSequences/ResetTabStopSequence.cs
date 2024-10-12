using AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.SequenceTypes;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    public class ResetTabStopSequence : CSISequence
    {
        public override char Command => 'W';

        public override void Execute(IAnsiContext context, string parameters)
        {
            context.Screen.ResetTabStops();
        }
    }
}