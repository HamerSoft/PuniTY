using HamerSoft.PuniTY.AnsiEncoding.SequenceTypes;
using HamerSoft.PuniTY.Logging;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    public class LinePositionAbsoluteSequence : CSISequence
    {
        public override char Command => 'd';

        public LinePositionAbsoluteSequence(ILogger logger) : base(logger)
        {
        }

        public override void Execute(IScreen screen, string parameters)
        {
            if (!TryParseInt(parameters, out var rowToSet))
            {
                Logger.LogWarning($"Cannot move cursor to row, invalid parameter: {parameters}. Int expected.");
                return;
            }

            screen.SetCursorPosition(new Position(rowToSet, screen.Cursor.Position.Column));
        }
    }
}