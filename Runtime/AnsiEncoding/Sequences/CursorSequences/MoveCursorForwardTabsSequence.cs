using System;
using HamerSoft.PuniTY.AnsiEncoding.SequenceTypes;
using ILogger = HamerSoft.PuniTY.Logging;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    public class MoveCursorForwardTabsSequence : CSISequence
    {
        public override char Command => 'I';

        public MoveCursorForwardTabsSequence(ILogger.ILogger logger) : base(logger)
        {
        }

        public override void Execute(IScreen screen, string parameters)
        {
            if (string.IsNullOrWhiteSpace(parameters))
                parameters = "1";

            if (!int.TryParse(parameters, out var offset))
                return;

            var tabStopSize = screen.ScreenConfiguration.TabStopSize;
            var currentTabStop = screen.Cursor.Position.Column / screen.ScreenConfiguration.TabStopSize;
            if (offset > screen.Columns - currentTabStop)
            {
                Logger.LogWarning(
                    $"Cannot move tabstops greater than max tabstops");
                return;
            }

            var targetColumn = Math.Clamp(Math.Abs((currentTabStop + offset) * tabStopSize), 1, screen.Columns);
            screen.SetCursorPosition(new Position(screen.Cursor.Position.Row, targetColumn));
        }
    }
}