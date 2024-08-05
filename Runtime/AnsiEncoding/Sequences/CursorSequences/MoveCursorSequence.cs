using HamerSoft.PuniTY.AnsiEncoding.SequenceTypes;
using ILogger = HamerSoft.PuniTY.Logging;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    public enum Direction
    {
        Up,
        Down,
        Forward,
        Back
    }

    internal abstract class MoveCursorSequence : CSISequence
    {
        public abstract override char Command { get; }
        protected abstract Direction Direction { get; }

        protected MoveCursorSequence(ILogger.ILogger logger) : base(logger)
        {
        }

        public override void Execute(IScreen screen, string parameters)
        {
            var cells = 1;
            if (string.IsNullOrWhiteSpace(parameters) || int.TryParse(parameters, out cells))
                screen.MoveCursor(cells, Direction);
            else
            {
                Logger.LogWarning($"Failed to parse move cursor command {Direction} => {parameters}");
            }
        }
    }
}