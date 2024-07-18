namespace HamerSoft.PuniTY.AnsiEncoding
{
    public class MoveCursorForwardTabsSequence : Sequence
    {
        public override char Command => 'I';

        public override void Execute(IScreen screen, string parameters)
        {
            if (string.IsNullOrWhiteSpace(parameters))
                parameters = "1";

            var maxTabs = screen.Columns / screen.ScreenConfiguration.TabStopSize;
            var currentTabStop = screen.Cursor.Position.Column / screen.ScreenConfiguration.TabStopSize;

            if (currentTabStop < maxTabs)
            {
                var cells = (currentTabStop + 1) * screen.ScreenConfiguration.TabStopSize -
                            screen.Cursor.Position.Column;
                screen.MoveCursor(
                    cells,
                    Direction.Forward);
            }
        }
    }
}