using AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.SequenceTypes;

namespace HamerSoft.PuniTY.AnsiEncoding.Characters
{
    public class RepeatPrecedingGraphicCharacter : CSISequence
    {
        public override char Command => 'b';

        public override void Execute(IAnsiContext context, string parameters)
        {
            if (!TryParseInt(parameters, out var repeats))
            {
                context.LogWarning($"Cannot repeat character, given: {parameters}. Int expected.");
                return;
            }

            var screen = context.Screen;
            if (screen.Cursor.Position == new Position(1, 1))
            {
                context.LogWarning(
                    "Cannot repeat preceding character because there is no preceding character at position (1,1).");
                return;
            }

            var precedingCharacterPosition = screen.Cursor.Position.Column == 1
                ? new Position(screen.Cursor.Position.Row - 1, screen.Columns)
                : new Position(screen.Cursor.Position.Row, screen.Cursor.Position.Column - 1);
            var precedingCharacter = screen.GetCharacter(precedingCharacterPosition);
            for (var i = 0; i < repeats; i++)
                screen.AddCharacter(precedingCharacter.Char);
        }
    }
}