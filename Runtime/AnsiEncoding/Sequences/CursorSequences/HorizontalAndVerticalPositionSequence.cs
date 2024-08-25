using HamerSoft.PuniTY.AnsiEncoding.SequenceTypes;
using HamerSoft.PuniTY.Logging;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    public class HorizontalAndVerticalPositionSequence : CSISequence
    {
        public override char Command => 'f';

        public HorizontalAndVerticalPositionSequence(ILogger logger) : base(logger)
        {
        }

        public override void Execute(IScreen screen, string parameters)
        {
            var rowAndColumns = GetCommandArguments(parameters, 2, 1);
            screen.SetCursorPosition(new Position(rowAndColumns[0], rowAndColumns[1]));
        }
    }
}