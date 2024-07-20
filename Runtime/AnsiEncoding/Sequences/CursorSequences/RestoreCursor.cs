using ILogger = HamerSoft.PuniTY.Logging;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    public class RestoreCursor : Sequence
    {
        public override char Command => 'u';

        public RestoreCursor(ILogger.ILogger logger) : base(logger)
        {
        }

        public override void Execute(IScreen screen, string _)
        {
            screen.RestoreCursor();
        }
    }

    public class RestoreCursorDec : RestoreCursor
    {
        public override char Command => '8';

        public RestoreCursorDec(ILogger.ILogger logger) : base(logger)
        {
        }
    }
}