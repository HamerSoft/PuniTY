namespace HamerSoft.PuniTY.AnsiEncoding.EraseSequences
{
    public class EraseDisplaySequence : Sequence
    {
        public override char Command => 'J';

        public override void Execute(IScreen screen, string parameters)
        {
            if (string.IsNullOrWhiteSpace(parameters))
            {
                parameters = "0";
            }

            if (!int.TryParse(parameters, out var number))
            {
                Logger.Warning($"Cannot Erase Display, failed to parse {parameters}");
                return;
            }

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
                    screen.SetCursorPosition(new Position(1,1));
                    break;
                case 3:
                    screen.Erase();
                    screen.ClearSaved();
                    break;
                default:
                    Logger.Error($"Cannot Erase Display, Argument Out of range {parameters}");
                    break;
            }
        }
    }
}