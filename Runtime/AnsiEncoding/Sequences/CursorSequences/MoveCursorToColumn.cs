using AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.SequenceTypes;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    public class MoveCursorToColumn : CSISequence
    {
        public override char Command => 'G';
        
        public override void Execute(IAnsiContext context, string parameters)
        {
            int column = 1;
            var screen = context.Screen;
            if (string.IsNullOrWhiteSpace(parameters) || int.TryParse(parameters, out column))
                screen.SetCursorPosition(new Position(screen.Cursor.Position.Row, column));
            else
            {
                context.LogWarning($"Failed to parse move cursor to column {parameters} command.");
            }
        }
    }
}