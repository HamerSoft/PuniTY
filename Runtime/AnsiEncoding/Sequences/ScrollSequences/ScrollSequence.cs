using HamerSoft.PuniTY.AnsiEncoding.SequenceTypes;
using ILogger = HamerSoft.PuniTY.Logging;

namespace HamerSoft.PuniTY.AnsiEncoding.ScrollSequences
{
    public abstract class ScrollSequence : CSISequence
    {
        public abstract override char Command { get; }
        protected abstract Direction Direction { get; }

        public ScrollSequence(ILogger.ILogger logger) : base(logger)
        {
        }

        public override void Execute(IScreen screen, string parameters)
        {
            if (string.IsNullOrWhiteSpace(parameters))
                parameters = "1";

            if (!int.TryParse(parameters, out var lines))
            {
                Logger.LogWarning($"Cannot scroll {Direction}, invalid parameter: {parameters}. Must be an integer.");
                return;
            }

            screen.Scroll(lines, Direction);
        }
    }
}