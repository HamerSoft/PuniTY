using AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.SequenceTypes;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    public class SetCursorPositionSequence : CSISequence
    {
        public override char Command => 'H';
        
        public override void Execute(IAnsiContext context, string parameters)
        {
            var values = GetCommandArguments(parameters, 2, 1);
            context.Screen.SetCursorPosition(new Position(values[0], values[1]));
        }
    }
}