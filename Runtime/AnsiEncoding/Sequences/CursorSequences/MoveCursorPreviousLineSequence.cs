using AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.SequenceTypes;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    internal class MoveCursorPreviousLineSequence : CSISequence
    {
        public override char Command => 'F';

        public override void Execute(IAnsiContext context, string parameters)
        {
            var cells = 1;
            var screen = context.Screen;
            if (string.IsNullOrWhiteSpace(parameters) || int.TryParse(parameters, out cells))
                screen.SetCursorPosition(new Position(screen.Cursor.Position.Row - cells, 1));
            else
            {
                context.LogWarning($"Failed to parse move cursor to beginning of the line {parameters} command.");
            }
        }
    }
}