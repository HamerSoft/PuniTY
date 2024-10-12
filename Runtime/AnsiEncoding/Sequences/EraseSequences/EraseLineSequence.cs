using AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.SequenceTypes;

namespace HamerSoft.PuniTY.AnsiEncoding.EraseSequences
{
    public class EraseLineSequence : CSISequence
    {
        public override char Command => 'K';
        
        public override void Execute(IAnsiContext context, string parameters)
        {
            if (string.IsNullOrWhiteSpace(parameters))
            {
                parameters = "0";
            }

            if (!int.TryParse(parameters, out var number))
            {
                context.LogWarning($"Cannot erase line, failed to parse {parameters}");
                return;
            }
var screen = context.Screen;
            switch (number)
            {
                case 0:
                    screen.Erase(screen.Cursor.Position, new Position(screen.Cursor.Position.Row, screen.Columns));
                    break;
                case 1:
                    screen.Erase(new Position(screen.Cursor.Position.Row, 1), screen.Cursor.Position);
                    break;
                case 2:
                    screen.Erase(new Position(screen.Cursor.Position.Row, 1),
                        new Position(screen.Cursor.Position.Row, screen.Columns));
                    break;
                default:
                    context.LogError($"Cannot Erase Display, Argument Out of range {parameters}");
                    break;
            }
        }
    }
}