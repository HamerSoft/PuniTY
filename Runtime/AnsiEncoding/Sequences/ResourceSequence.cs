using AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.SequenceTypes;
using HamerSoft.PuniTY.AnsiEncoding.TerminalModes;
using NUnit.Framework.Internal;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    public class ResourceSequence : CSISequence
    {
        private const int InvalidArgument = -1;
        private const char SetResource = '>';
        private const char DecSpecificIndicator = '?';

        public override char Command => 'p';

        public override void Execute(IAnsiContext context, string parameters)
        {
            if (string.IsNullOrWhiteSpace(parameters))
            {
                context.LogWarning($"Failed to executed {nameof(GetType)}, no parameters given. Skipping command");
                return;
            }

            var paramsToParse = parameters.StartsWith(SetResource) ||
                                parameters.StartsWith(DecSpecificIndicator)
                ? parameters.Substring(1, parameters.Length - 1)
                : parameters;

            if (!TryParseInt(paramsToParse, out var argument, "-1"))
            {
                context.LogWarning($"Failed to parse argument {nameof(GetType)}, no parameters invalid. Int Expected.");
                return;
            }

            if (InvalidArgument == argument)
            {
                context.LogWarning($"Failed to parse argument {nameof(GetType)}, parameter invalid. Int Expected.");
                return;
            }

            var screen = context.Screen;
            if (parameters.StartsWith(SetResource))
                SetPointerResourceMode(context.TerminalModeContext, argument);
            else if (parameters.StartsWith(DecSpecificIndicator))
                ExecuteDecSpecific(screen, argument);
            else
                ExecuteNormal(screen, argument);
        }

        private void ExecuteNormal(IScreen screen, int argument)
        {
        }

        private void ExecuteDecSpecific(IScreen screen, int argument)
        {
        }

        private void SetPointerResourceMode(IPointerable pointerable, int argument)
        {
            switch (argument)
            {
                case 0:
                    pointerable.SetPointerMode(PointerMode.NeverHide);
                    break;
                case 1:
                    pointerable.SetPointerMode(PointerMode.HideIfNotTracking);
                    break;
                case 2:
                    pointerable.SetPointerMode(PointerMode.AlwaysHideInWindow);
                    break;
                case 3:
                    pointerable.SetPointerMode(PointerMode.AlwaysHide);
                    break;
            }
        }
    }
}