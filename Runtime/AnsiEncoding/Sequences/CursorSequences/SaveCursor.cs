using AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.SequenceTypes;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    public class SaveCursor : CSISequence
    {
        public override char Command => 's';

        public override void Execute(IAnsiContext context, string _)
        {
            context.Screen.SaveCursor();
        }
    }

    public class SaveCursorDec : SaveCursor
    {
        public override SequenceType SequenceType => SequenceType.ESC;
        public override char Command => '7';
    }
}