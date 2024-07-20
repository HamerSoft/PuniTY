using ILogger = HamerSoft.PuniTY.Logging;

namespace HamerSoft.PuniTY.AnsiEncoding.EraseSequences
{
    public class EraseLineSequence : Sequence
    {
        public override char Command => 'K';

        public EraseLineSequence(ILogger.ILogger logger) : base(logger)
        {
        }

        public override void Execute(IScreen screen, string parameters)
        {
            if (string.IsNullOrWhiteSpace(parameters))
            {
                parameters = "0";
            }

            if (!int.TryParse(parameters, out var number))
            {
                Logger.LogWarning($"Cannot erase line, failed to parse {parameters}");
                return;
            }

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
                    Logger.LogError($"Cannot Erase Display, Argument Out of range {parameters}");
                    break;
            }
        }
    }
}