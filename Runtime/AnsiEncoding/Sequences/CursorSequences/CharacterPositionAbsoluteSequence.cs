using AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.SequenceTypes;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    public class CharacterPositionAbsoluteSequence : CSISequence
    {
        public override char Command => '`';

        public override void Execute(IAnsiContext context, string parameters)
        {
            if (!TryParseInt(parameters, out var targetColumn))
            {
                context.LogWarning($"Cannot move cursor to column given: {parameters}. Int expected");
                return;
            }

            var screen = context.Screen;
            if (targetColumn < 1 || targetColumn > screen.Columns)
            {
                context.LogWarning(
                    $"Cannot move cursor to column given: {targetColumn}. Column must be greater than 0 and smaller than {screen.Columns + 1}.");
                return;
            }

            screen.SetCursorPosition(new Position(screen.Cursor.Position.Row, targetColumn));
        }
    }
}