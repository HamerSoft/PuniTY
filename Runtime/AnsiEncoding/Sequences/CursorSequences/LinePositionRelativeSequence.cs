using HamerSoft.PuniTY.AnsiEncoding.SequenceTypes;
using HamerSoft.PuniTY.Logging;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    public class LinePositionRelativeSequence : CSISequence
    {
        public override char Command => 'e';

        public LinePositionRelativeSequence(ILogger logger) : base(logger)
        {
        }

        public override void Execute(IScreen screen, string parameters)
        {
            if (!TryParseInt(parameters, out var rowToAdd))
            {
                Logger.LogWarning($"Cannot add rows to cursor, invalid parameter: {parameters}. Int expected.");
                return;
            }

            screen.SetCursorPosition(screen.Cursor.Position.Add(screen, new Position(rowToAdd, 0)));
        }
    }
}