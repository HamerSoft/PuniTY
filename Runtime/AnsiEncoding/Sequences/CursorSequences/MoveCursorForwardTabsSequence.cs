using ILogger = HamerSoft.PuniTY.Logging;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    public class MoveCursorForwardTabsSequence : Sequence
    {
        public override char Command => 'I';

        public MoveCursorForwardTabsSequence(ILogger.ILogger logger) : base(logger)
        {
        }

        public override void Execute(IScreen screen, string parameters)
        {
            if (string.IsNullOrWhiteSpace(parameters))
                parameters = "1";

            if (!int.TryParse(parameters, out var desiredTabStop))
                return;

            var maxTabs = screen.Columns / screen.ScreenConfiguration.TabStopSize;
            if (desiredTabStop > maxTabs)
            {
                Logger.LogWarning(
                    $"Cannot move to tab stop greater than max! Max: {maxTabs}, Desired: {desiredTabStop}");
                return;
            }

            var currentTabStop = screen.Cursor.Position.Column / screen.ScreenConfiguration.TabStopSize;
            var desiredPosition = desiredTabStop * screen.ScreenConfiguration.TabStopSize;
            if (currentTabStop > desiredTabStop || desiredPosition < screen.Cursor.Position.Column)
            {
                Logger.LogWarning("Cannot move cursor backward, with forward tab command.");
                return;
            }

            var cells = desiredPosition - screen.Cursor.Position.Column;
            screen.MoveCursor(
                cells,
                Direction.Forward);
        }
    }
}