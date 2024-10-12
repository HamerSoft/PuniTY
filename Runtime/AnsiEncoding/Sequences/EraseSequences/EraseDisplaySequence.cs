using AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.SequenceTypes;

namespace HamerSoft.PuniTY.AnsiEncoding.EraseSequences
{
    public class EraseDisplaySequence : CSISequence
    {
        public override char Command => 'J';

        public override void Execute(IAnsiContext context, string parameters)
        {
            if (string.IsNullOrWhiteSpace(parameters))
            {
                parameters = "0";
            }

            if (!int.TryParse(parameters, out var number))
            {
                context.LogWarning($"Cannot Erase Display, failed to parse {parameters}");
                return;
            }

            var screen = context.Screen;
            switch (number)
            {
                case 0:
                    screen.Erase(screen.Cursor.Position);
                    break;
                case 1:
                    screen.Erase(null, screen.Cursor.Position);
                    break;
                case 2:
                    screen.Erase();
                    screen.SetCursorPosition(new Position(1, 1));
                    break;
                case 3:
                    screen.Erase();
                    screen.ClearSaved();
                    screen.SetCursorPosition(new Position(1, 1));
                    break;
                default:
                    context.LogError($"Cannot erase display, Argument Out of range {parameters}");
                    break;
            }
        }
    }
}