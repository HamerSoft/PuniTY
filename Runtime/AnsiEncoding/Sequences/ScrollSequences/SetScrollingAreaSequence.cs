using AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.SequenceTypes;

namespace HamerSoft.PuniTY.AnsiEncoding.ScrollSequences
{
    public class SetScrollingAreaSequence : CSISequence
    {
        private const char PrivateModeValues = '?';
        public override char Command => 'r';

        public override void Execute(IAnsiContext context, string parameters)
        {
            if (string.IsNullOrWhiteSpace(parameters))
            {
                context.LogWarning("Setting scroll-area of 0,0 is invalid... Skipping command.");
                return;
            }

            if (parameters.StartsWith(PrivateModeValues))
            {
                var parametersToParse = parameters.Substring(0, parameters.Length - 1);
                if (TryParseInt(parametersToParse, out var argument, "-1"))
                {
                    context.LogWarning("Reset Private Dec Mode not implemented.");
                }
                else
                {
                    context.LogWarning(
                        $"Cannot Reset Private DEC Mode values, invalid argument {parametersToParse}. Int Expected.");
                    return;
                }
            }
            else
            {
                var arguments = GetCommandArguments(parameters, 2, -1);
                var top = arguments[0];
                var bottom = arguments[1];
                if (bottom <= 0 || top <= 0 || bottom <= top)
                {
                    context.LogWarning("Cannot set ScrollArea, invalid dimensions.");
                    return;
                }

                context.Screen.SetScrollingArea(top, bottom);
            }
        }
    }
}