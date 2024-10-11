using AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.SequenceTypes;
using ILogger = HamerSoft.PuniTY.Logging.ILogger;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    public class ResetTabStopSequence : CSISequence
    {
        public override char Command => 'W';

        public ResetTabStopSequence(ILogger logger) : base(logger)
        {
        }

        public override void Execute(IAnsiContext context, string parameters)
        {
            context.Screen.ResetTabStops();
        }
    }
}