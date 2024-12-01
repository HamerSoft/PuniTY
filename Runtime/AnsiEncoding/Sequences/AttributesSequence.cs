using System;
using AnsiEncoding;
using HamerSoft.PuniTY.AnsiEncoding.SequenceTypes;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    public class AttributesSequence : CSISequence
    {
        private const char CursorStyle = ' ';
        private const char CharacterProtection = '"';
        private const char PopVideoAttributes = '#';
        private const int InvalidArgument = -1;
        public override char Command => 'q';

        public override void Execute(IAnsiContext context, string parameters)
        {
            if (string.IsNullOrWhiteSpace(parameters))
            {
                context.LogWarning($"Failed to executed {GetType()}, no parameters given. Skipping command");
                return;
            }

            var paramsToParse = parameters.EndsWith(CursorStyle)
                                || parameters.EndsWith(CharacterProtection)
                ? parameters.Substring(0, Math.Clamp(parameters.Length - 1, 0, 4))
                : parameters;

            if (paramsToParse == $"{PopVideoAttributes}")
            {
                context.LogWarning("Pop Video Attributes not implemented!");
                return;
            }

            if (string.IsNullOrWhiteSpace(paramsToParse))
                paramsToParse = "0";

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

            if (parameters.EndsWith(CursorStyle))
                ExecuteSetCursorMode(context, argument);
            else if (parameters.EndsWith(CharacterProtection))
                ExecuteCharacterProtection(context, argument);
            else
                ExecuteNormal(context, argument);
        }

        private void ExecuteCharacterProtection(IAnsiContext context, int argument)
        {
            switch (argument)
            {
                case 0:
                case 2:
                    context.Screen.SetCharacterProtection(false);
                    break;
                case 1:
                    context.Screen.SetCharacterProtection(true);
                    break;
                default:
                    context.LogWarning($"Cannot set character protection, unknown argument{argument}.");
                    break;
            }
        }

        private void ExecuteSetCursorMode(IAnsiContext context, int argument)
        {
            switch (argument)
            {
                case 0:
                case 1:
                    context.TerminalModeContext.SetCursorMode(CursorMode.BlinkingBlock);
                    break;
                case 2:
                    context.TerminalModeContext.SetCursorMode(CursorMode.SteadyBlock);
                    break;
                case 3:
                    context.TerminalModeContext.SetCursorMode(CursorMode.BlinkingUnderline);
                    break;
                case 4:
                    context.TerminalModeContext.SetCursorMode(CursorMode.SteadyUnderLine);
                    break;
                case 5:
                    context.TerminalModeContext.SetCursorMode(CursorMode.BlinkingBar);
                    break;
                case 6:
                    context.TerminalModeContext.SetCursorMode(CursorMode.SteadyBar);
                    break;
                default:
                    context.LogWarning($"Cannot set CursorStyle, Unknown argument: {argument}.");
                    break;
            }
        }

        private void ExecuteNormal(IAnsiContext context, int argument)
        {
            switch (argument)
            {
                case 0:
                    context.LogWarning("Clear all LEDS, not implemented");
                    break;
                case 1:
                    context.LogWarning("Light Num Lock, not implemented");
                    break;
                case 2:
                    context.LogWarning("Light Caps Lock, not implemented");
                    break;
                case 3:
                    context.LogWarning("Light Scroll Lock, not implemented");
                    break;
                case 21:
                    context.LogWarning("Extinguish Num Lock, not implemented");
                    break;
                case 22:
                    context.LogWarning("Extinguish Caps Lock, not implemented");
                    break;
                case 23:
                    context.LogWarning("Extinguish Scroll Lock, not implemented");
                    break;
                default:
                    context.LogWarning($"Cannot set LEDs, unknown argument: {argument}");
                    break;
            }
        }
    }
}