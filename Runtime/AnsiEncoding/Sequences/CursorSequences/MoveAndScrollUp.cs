using AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.SequenceTypes;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    public class MoveAndScrollUp : ESCSequence
    {
        public override char Command => 'M';

        public override void Execute(IAnsiContext context, string _)
        {
            var screen = context.Screen;
            screen.SetCursorPosition(new Position(screen.Cursor.Position.Row - 1, screen.Cursor.Position.Column));
        }
    }
}