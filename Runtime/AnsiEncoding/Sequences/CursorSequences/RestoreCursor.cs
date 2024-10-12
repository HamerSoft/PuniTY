using AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.SequenceTypes;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    public class RestoreCursor : CSISequence
    {
        public override char Command => 'u';

        public override void Execute(IAnsiContext context, string _)
        {
            context.Screen.RestoreCursor();
        }
    }

    public class RestoreCursorDec : RestoreCursor
    {
        public override SequenceType SequenceType => SequenceType.ESC;
        public override char Command => '8';
    }
}