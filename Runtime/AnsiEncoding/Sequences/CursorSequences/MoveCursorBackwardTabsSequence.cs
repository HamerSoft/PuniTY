using HamerSoft.PuniTY.AnsiEncoding.SequenceTypes;
using ILogger = HamerSoft.PuniTY.Logging;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    public class MoveCursorBackwardTabsSequence : CSISequence
    {
        public override char Command => 'Z';

        public MoveCursorBackwardTabsSequence(ILogger.ILogger logger) : base(logger)
        {
        }

        public override void Execute(IScreen screen, string parameters)
        {
            if (string.IsNullOrWhiteSpace(parameters))
                parameters = "1";

            if (!int.TryParse(parameters, out var offset))
                return;

            var currentTabStop = screen.Cursor.Position.Column / screen.ScreenConfiguration.TabStopSize;
            if (offset > currentTabStop)
            {
                Logger.LogWarning(
                    $"Cannot move tabstops smaller than 0");
                return;
            }

            var targetColumn = screen.TabStopToColumn(screen.GetCurrentTabStop(screen.Cursor.Position.Column) - offset);
            screen.SetCursorPosition(new Position(screen.Cursor.Position.Row, targetColumn));
        }
    }
}