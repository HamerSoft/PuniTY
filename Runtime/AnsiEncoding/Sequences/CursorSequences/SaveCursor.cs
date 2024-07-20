using ILogger = HamerSoft.PuniTY.Logging;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    public class SaveCursor : Sequence
    {
        public override char Command => 's';

        public SaveCursor(ILogger.ILogger logger) : base(logger)
        {
        }

        public override void Execute(IScreen screen, string _)
        {
            screen.SaveCursor();
        }
    }

    public class SaveCursorDec : SaveCursor
    {
        public override char Command => '7';

        public SaveCursorDec(ILogger.ILogger logger) : base(logger)
        {
        }
    }
}