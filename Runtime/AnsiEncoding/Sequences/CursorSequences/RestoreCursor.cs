using HamerSoft.PuniTY.AnsiEncoding.SequenceTypes;
using ILogger = HamerSoft.PuniTY.Logging;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    public class RestoreCursor : CSISequence
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
        public override SequenceType SequenceType => SequenceType.ESC;
        public override char Command => '8';

        public RestoreCursorDec(ILogger.ILogger logger) : base(logger)
        {
        }
    }
}