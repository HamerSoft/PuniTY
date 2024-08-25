using HamerSoft.PuniTY.AnsiEncoding.SequenceTypes;
using HamerSoft.PuniTY.Logging;

namespace HamerSoft.PuniTY.AnsiEncoding.EraseSequences
{
    public class TabClearSequence : CSISequence
    {
        private const int Zero = 0;
        private const int Three = 3;
        public override char Command => 'g';

        public TabClearSequence(ILogger logger) : base(logger)
        {
        }

        public override void Execute(IScreen screen, string parameters)
        {
            if (!TryParseInt(parameters, out var parameter, "0"))
            {
                Logger.LogWarning("Cannot clear column / all, invalid parameter. Int Expected.");
                return;
            }

            switch (parameter)
            {
                case Zero:
                    screen.ClearTabStop(screen.Cursor.Position.Column);
                    break;
                case Three:
                    screen.ClearTabStop(null);
                    break;
                default:
                    Logger.LogWarning(
                        $"TabClear only accepts parameters '0' or '3'. Given: {parameter}. Skipping command.");
                    break;
            }
        }
    }
}