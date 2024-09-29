using HamerSoft.PuniTY.AnsiEncoding.SequenceTypes;
using HamerSoft.PuniTY.Logging;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    public class ResourceSequence : CSISequence
    {
        private const int InvalidArgument = -1;
        private const char SetResource = '>';
        private const char DecSpecificIndicator = '?';

        public override char Command => 'p';

        public ResourceSequence(ILogger logger) : base(logger)
        {
        }

        public override void Execute(IScreen screen, string parameters)
        {
            if (string.IsNullOrWhiteSpace(parameters))
            {
                Logger.LogWarning($"Failed to executed {nameof(GetType)}, no parameters given. Skipping command");
                return;
            }

            var paramsToParse = parameters.StartsWith(SetResource) ||
                                parameters.StartsWith(DecSpecificIndicator)
                ? parameters.Substring(1, parameters.Length - 1)
                : parameters;

            if (!TryParseInt(paramsToParse, out var argument, "-1"))
            {
                Logger.LogWarning($"Failed to parse argument {nameof(GetType)}, no parameters invalid. Int Expected.");
                return;
            }

            if (InvalidArgument == argument)
            {
                Logger.LogWarning($"Failed to parse argument {nameof(GetType)}, parameter invalid. Int Expected.");
                return;
            }

            if (parameters.StartsWith(SetResource))
                SetPointerResourceMode(screen, argument);
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

        private void SetPointerResourceMode(IScreen screen, int argument)
        {
            switch (argument)
            {
                case 0:
                    screen.SetPointerMode(PointerMode.NeverHide);
                    break;
                case 1:
                    screen.SetPointerMode(PointerMode.HideIfNotTracking);
                    break;
                case 2:
                    screen.SetPointerMode(PointerMode.AlwaysHideInWindow);
                    break;
                case 3:
                    screen.SetPointerMode(PointerMode.AlwaysHide);
                    break;
            }
        }
    }
}