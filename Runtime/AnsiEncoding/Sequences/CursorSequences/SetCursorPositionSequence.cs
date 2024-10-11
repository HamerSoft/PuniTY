using AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.SequenceTypes;
using ILogger = HamerSoft.PuniTY.Logging;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    public class SetCursorPositionSequence : CSISequence
    {
        public override char Command => 'H';

        public SetCursorPositionSequence(ILogger.ILogger logger) : base(logger)
        {
        }

        public override void Execute(IAnsiContext context, string parameters)
        {
            var values = GetCommandArguments(parameters, 2, 1);
            context.Screen.SetCursorPosition(new Position(values[0], values[1]));
        }
    }
}