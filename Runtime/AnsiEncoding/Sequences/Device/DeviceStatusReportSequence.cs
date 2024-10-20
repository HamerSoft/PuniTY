using AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.SequenceTypes;

namespace HamerSoft.PuniTY.AnsiEncoding.Device
{
    public class DeviceStatusReportSequence : CSISequence
    {
        private const string Escape = "\x001b[";
        private const int InvalidArgument = -1;
        private const char DisableKeyModifierIndicator = '>';
        private const char DecSpecificIndicator = '?';

        public override char Command => 'n';

        public override void Execute(IAnsiContext context, string parameters)
        {
            if (string.IsNullOrWhiteSpace(parameters))
            {
                context.LogWarning($"Failed to executed {nameof(GetType)}, no parameters given. Skipping command");
                return;
            }

            var paramsToParse = parameters.StartsWith(DisableKeyModifierIndicator) ||
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

            context.LogWarning(
                "DSR requested: not officially supported, create GH issue for feature request or make a PR.");
            if (parameters.StartsWith(DisableKeyModifierIndicator))
                DisableKeyModifiers(context, argument);
            else if (parameters.StartsWith(DecSpecificIndicator))
                ExecuteDecSpecific(context, argument);
            else
                ExecuteNormal(context, argument);
        }

        private void ExecuteNormal(IAnsiContext context, int argument)
        {
            var screen = context.Screen;
            var transmitter = context.InputTransmitter;
            switch (argument)
            {
                case 5:
                    transmitter.Transmit($"{Escape}0n");
                    break;
                case 6:
                    transmitter.Transmit(
                        $"{Escape}{screen.Cursor.Position.Row};{screen.Cursor.Position.Column};R");
                    break;
            }
        }

        private void ExecuteDecSpecific(IAnsiContext context, int argument)
        {
            var transmitter = context.InputTransmitter;
            var screen = context.Screen;
            switch (argument)
            {
                case 6:
                    transmitter.Transmit(
                        $"{Escape}{screen.Cursor.Position.Row};{screen.Cursor.Position.Column};R");
                    break;
                case 15:
                    context.LogWarning("Printer not implemented, so returning not ready.");
                    // Report Printer status.  The response is
                    // CSI ? 1 0 n  (ready).  or
                    // CSI ? 1 1 n  (not ready).
                    transmitter.Transmit($"{Escape}11n");
                    break;
                case 25:
                    // how to lock User-Defined Keys (UDK)?
                    // Report UDK status.  The response is
                    // CSI ? 2 0 n  (unlocked) or
                    // CSI ? 2 1 n  (locked).
                    transmitter.Transmit($"{Escape}20n");
                    break;
                case 26:
                    // How to check for errors?
                    transmitter.Transmit($"{Escape}27;0n");
                    break;
                case 55:
                    //Report Locator status.  The response is CSI ? 5
                    // 0 n  Locator available, if compiled-in, or CSI ? 5 3 n No
                    // Locator, if not.
                    // Ps = 5 6  ⇒  Report Locator type.  The response is CSI ? 5 7
                    // ; 1 n  Mouse, if compiled-in, or CSI ? 5 7 ; 0 n  Cannot
                    // identify, if not.

                    // How to detect, in this case return cannot identify for now
                    transmitter.Transmit($"{Escape}57n");
                    break;
                case 62:
                    // How to check? Returning default 2048 for now
                    transmitter.Transmit($"{Escape}" + "2048*{");
                    break;
                case 63:
                    // How to check?
                    // returning 0 for now
                    transmitter.Transmit($"{Escape}DCS0~0ST");
                    break;
                case 75:
                    // how to check
                    // returning OK for now
                    transmitter.Transmit($"{Escape}70n");
                    break;
                case 83:
                    // how to check
                    // returning no for now
                    // 82 is yes
                    // 83 is no
                    transmitter.Transmit($"{Escape}83n");
                    break;
            }
        }

        private void DisableKeyModifiers(IAnsiContext context, int argument)
        {
            context.LogWarning("Disable Key Modifier Options not supported! Skipping command.");
        }
    }
}