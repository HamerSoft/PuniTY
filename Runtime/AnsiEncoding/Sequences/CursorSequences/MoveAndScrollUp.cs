using AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.SequenceTypes;
using ILogger = HamerSoft.PuniTY.Logging;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    public class MoveAndScrollUp : ESCSequence
    {
        public override char Command => 'M';

        public MoveAndScrollUp(ILogger.ILogger logger) : base(logger)
        {
        }

        public override void Execute(IAnsiContext context, string _)
        {
            var screen = context.Screen;
            screen.SetCursorPosition(new Position(screen.Cursor.Position.Row - 1, screen.Cursor.Position.Column));
        }
    }
}