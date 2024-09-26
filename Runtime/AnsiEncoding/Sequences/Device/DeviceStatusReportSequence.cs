using ILogger = HamerSoft.PuniTY.Logging;

namespace HamerSoft.PuniTY.AnsiEncoding.Device
{
    public class DeviceStatusReportSequence : TransmitSequence
    {
        private const int InvalidArgument = -1;
        private const char DisableKeyModifierIndicator = '>';
        private const char DecSpecificIndicator = '?';

        public override char Command => 'n';

        public DeviceStatusReportSequence(ILogger.ILogger logger) : base(logger)
        {
        }

        public override void Execute(IScreen screen, string parameters)
        {
            if (string.IsNullOrWhiteSpace(parameters))
            {
                Logger.LogWarning($"Failed to executed {nameof(GetType)}, no parameters given. Skipping command");
                return;
            }

            var paramsToParse = parameters.StartsWith(DisableKeyModifierIndicator) ||
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

            if (parameters.StartsWith(DisableKeyModifierIndicator))
                DisableKeyModifiers(screen, argument);
            else if (parameters.StartsWith(DecSpecificIndicator))
                ExecuteDecSpecific(screen, argument);
            else
                ExecuteNormal(screen, argument);
        }

        private void ExecuteNormal(IScreen screen, int argument)
        {
            switch (argument)
            {
                case 5:
                    screen.Transmit(ToBytes($"{Escape}0n"));
                    break;
                case 6:
                    screen.Transmit(ToBytes($"{Escape}{screen.Cursor.Position.Row};{screen.Cursor.Position.Column};R"));
                    break;
            }
        }

        private void ExecuteDecSpecific(IScreen screen, int argument)
        {
            throw new System.NotImplementedException();
        }

        private void DisableKeyModifiers(IScreen screen, int argument)
        {
            throw new System.NotImplementedException();
        }
    }
}