using AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.SequenceTypes;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    public class HorizontalAndVerticalPositionSequence : CSISequence
    {
        public override char Command => 'f';

        public override void Execute(IAnsiContext context, string parameters)
        {
            var rowAndColumns = GetCommandArguments(parameters, 2, 1);
            context.Screen.SetCursorPosition(new Position(rowAndColumns[0], rowAndColumns[1]));
        }
    }
}