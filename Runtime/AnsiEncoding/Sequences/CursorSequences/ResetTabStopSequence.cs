using HamerSoft.PuniTY.AnsiEncoding.SequenceTypes;
using ILogger = HamerSoft.PuniTY.Logging.ILogger;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    public class ResetTabStopSequence : CSISequence
    {
        private const int DefaultTabStops = 8;
        public override char Command => 'W';

        public ResetTabStopSequence(ILogger logger) : base(logger)
        {
        }

        public override void Execute(IScreen screen, string parameters)
        {
            Logger.LogWarning("Resetting Tab stops not supported.");
        }
    }
}