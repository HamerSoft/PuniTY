using AnsiEncoding;
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

        public override void Execute(IAnsiContext context, string parameters)
        {
            var rowAndColumns = GetCommandArguments(parameters, 2, 1);
            context.Screen.SetCursorPosition(new Position(rowAndColumns[0], rowAndColumns[1]));
        }
    }
}