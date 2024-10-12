using AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.SequenceTypes;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    public class LinePositionRelativeSequence : CSISequence
    {
        public override char Command => 'e';
        
        public override void Execute(IAnsiContext context, string parameters)
        {
            if (!TryParseInt(parameters, out var rowToAdd))
            {
                context.LogWarning($"Cannot add rows to cursor, invalid parameter: {parameters}. Int expected.");
                return;
            }

            var screen = context.Screen;
            screen.SetCursorPosition(screen.Cursor.Position.Add(screen, new Position(rowToAdd, 0)));
        }
    }
}