using ILogger = HamerSoft.PuniTY.Logging;

namespace HamerSoft.PuniTY.AnsiEncoding.ScrollSequences
{
    public class ScrollDownSequence : ScrollSequence
    {
        public override char Command => 'T';
        protected override Direction Direction => Direction.Down;

        public ScrollDownSequence(ILogger.ILogger logger) : base(logger)
        {
        }

        public override void Execute(IScreen screen, string parameters)
        {
            if (parameters.Contains(';'))
            {
                Logger.LogWarning("Mouse tracking not implemented! Skipping command!");
                return;
            }

            base.Execute(screen, parameters);
        }
    }
}