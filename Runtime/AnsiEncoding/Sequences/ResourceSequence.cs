using System;
using AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.SequenceTypes;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    public class ResourceSequence : CSISequence
    {
        private const int InvalidArgument = -1;
        private const char SetResource = '>';
        private const char DecSpecificIndicator = '?';
        private const char SoftTerminalReset = '!';
        private const char ConformanceLevel = '"'; // not supported due to '"' in command
        private const char RequestANSImode = '$';

        public override char Command => 'p';

        public override void Execute(IAnsiContext context, string parameters)
        {
            if (string.IsNullOrWhiteSpace(parameters))
            {
                context.LogWarning($"Failed to executed {GetType()}, no parameters given. Skipping command");
                return;
            }

            var paramsToParse = parameters.StartsWith(SetResource) ||
                                parameters.StartsWith(DecSpecificIndicator) ||
                                parameters.StartsWith(SoftTerminalReset)
                ? parameters.Substring(1, parameters.Length - 1)
                : parameters.EndsWith(ConformanceLevel) ||
                  parameters.EndsWith(RequestANSImode)
                    ? parameters.Substring(0, Math.Clamp(parameters.Length - 1, 0, 4))
                    : parameters;
            bool isPrivateANSIModeRequest = paramsToParse.EndsWith(RequestANSImode);
            paramsToParse = isPrivateANSIModeRequest
                ? paramsToParse.Substring(0, Math.Clamp(paramsToParse.Length - 1, 0, 4))
                : paramsToParse;

            if (!TryParseInt(paramsToParse, out var argument, "-1"))
            {
                context.LogWarning($"Failed to parse argument {GetType()}, no parameters invalid. Int Expected.");
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
                if (isPrivateANSIModeRequest)
                    ExecuteRequestPrivateAnsiMode(context, argument);
                else
                    ExecuteDecSpecific(screen, argument);
            else if (parameters.StartsWith(SoftTerminalReset))
                ExecuteSoftTerminalReset(context);
            else if (parameters.EndsWith(RequestANSImode))
                ExecuteRequestAnsiMode(context, argument);
            else
                ExecuteNormal(screen, argument);
        }

        private void ExecuteRequestPrivateAnsiMode(IAnsiContext context, int argument)
        {
            context.LogWarning("Request Private ANSI mode, Not implemented.");
        }

        private void ExecuteRequestAnsiMode(IAnsiContext context, int argument)
        {
            switch (argument)
            {
                case 0:
                    context.LogWarning("Request ANSI mode - 0 Not Recognized, Not implemented.");
                    break;
                case 1:
                    context.LogWarning("Request ANSI mode - 1 set, Not implemented.");
                    break;
                case 2:
                    context.LogWarning("Request ANSI mode - 2 reset, Not implemented.");
                    break;
                case 3:
                    context.LogWarning("Request ANSI mode - 3 permanently set, Not implemented.");
                    break;
                case 4:
                    context.LogWarning("Request ANSI mode - 4 permanently reset, Not implemented.");
                    break;
            }
        }

        /// <summary>
        /// Soft reset terminal
        /// </summary>
        /// <remarks>The Soft Terminal Reset (DECSTR) command resets various terminal settings to their default states without clearing the screen or affecting the terminal's communication settings. This is useful for restoring the terminal to a known state without disrupting the current session. Here are some of the settings that are typically reset:
        /// Character Sets: Resets the character sets to their default mappings.
        /// Modes: Resets various modes such as insert/replace mode, origin mode, and wraparound mode.
        /// Attributes: Clears any text attributes like bold, underline, or reverse video.
        /// Tabs: Resets tab stops to their default positions.
        /// Cursor: Resets the cursor to its default state.</remarks>
        /// <param name="context"></param>
        private void ExecuteSoftTerminalReset(IAnsiContext context)
        {
            context.LogWarning(
                "Soft terminal reset (DECSTR) not implemented: please create a feature request with details.");
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