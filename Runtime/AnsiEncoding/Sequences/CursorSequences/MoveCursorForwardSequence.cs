using ILogger = HamerSoft.PuniTY.Logging;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    internal class MoveCursorForwardSequence : MoveCursorSequence
    {
        public override char Command => 'C';
        protected override Direction Direction => Direction.Forward;

        public MoveCursorForwardSequence(ILogger.ILogger logger) : base(logger)
        {
        }
    }
}