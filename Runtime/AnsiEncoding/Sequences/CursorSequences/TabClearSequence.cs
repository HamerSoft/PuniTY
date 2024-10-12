using AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.SequenceTypes;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    public class TabClearSequence : CSISequence
    {
        private const int Zero = 0;
        private const int Three = 3;
        public override char Command => 'g';

        public override void Execute(IAnsiContext context, string parameters)
        {
            if (!TryParseInt(parameters, out var parameter, "0"))
            {
                context.LogWarning("Cannot clear column / all, invalid parameter. Int Expected.");
                return;
            }

            var screen = context.Screen;
            switch (parameter)
            {
                case Zero:
                    screen.ClearTabStop(screen.Cursor.Position.Column);
                    break;
                case Three:
                    screen.ClearTabStop(null);
                    break;
                default:
                    context.LogWarning(
                        $"TabClear only accepts parameters '0' or '3'. Given: {parameter}. Skipping command.");
                    break;
            }
        }
    }
}